using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System.Linq;

namespace Xam.Utility.Extensions
{
	public static class RewiredExtensions
	{
		private const string k_enabledControlsRuleSetTag = "Controls";

		public static void CreateMapActivationRules( this Player player )
		{
			ControllerMapEnabler.RuleSet enabledControlsRuleSet = new ControllerMapEnabler.RuleSet()
			{
				enabled = true,
				tag = k_enabledControlsRuleSetTag,
				rules = new List<ControllerMapEnabler.Rule>()
			};

			IEnumerable<ControllerMap> allMaps = player.controllers.maps.GetAllMaps();
			foreach ( ControllerMap map in allMaps )
			{
				enabledControlsRuleSet.rules.Add( new ControllerMapEnabler.Rule()
				{
					enable = map.enabled,
					categoryId = map.categoryId,
					controllerSetSelector = ControllerSetSelector.SelectAll()
				} );
			}

			player.controllers.maps.mapEnabler.ruleSets.Clear();
			player.controllers.maps.mapEnabler.ruleSets.Add( enabledControlsRuleSet );
			player.controllers.maps.mapEnabler.Apply();
		}

		public static void SetControlsActive( this Player player, int categoryId, bool isActive )
		{
			ControllerMapEnabler.RuleSet enabledControlsRuleSet = player.controllers.maps.mapEnabler.ruleSets.Find( 
				queryRuleSet => queryRuleSet.tag == k_enabledControlsRuleSetTag 
			);
			
			foreach ( ControllerMapEnabler.Rule rule in enabledControlsRuleSet.rules )
			{
				if ( rule.categoryId == categoryId )
				{
					rule.enable = isActive;
				}
			}

			player.controllers.maps.mapEnabler.Apply();
		}

		public static void SetAllControlsActive( this Player player, bool isActive )
		{
			ControllerMapEnabler.RuleSet enabledControlsRuleSet = player.controllers.maps.mapEnabler.ruleSets.Find(
				queryRuleSet => queryRuleSet.tag == k_enabledControlsRuleSetTag
			);

			foreach ( ControllerMapEnabler.Rule rule in enabledControlsRuleSet.rules )
			{
				rule.enable = isActive;
			}

			player.controllers.maps.mapEnabler.Apply();
		}
	}
}