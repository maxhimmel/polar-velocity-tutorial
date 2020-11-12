using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Xam.PostProcessing
{
	[System.Serializable]
	[PostProcess( typeof( PixelateRenderer ), PostProcessEvent.BeforeStack, "Xam/Pixelate", false )]
	public sealed class Pixelate : PostProcessEffectSettings
	{
		public Vector2Parameter ScreenResolution = new Vector2Parameter() { value = new Vector2( 640, 360 ) };
		public Vector2Parameter CellSize = new Vector2Parameter() { value = new Vector2( 4, 4 ) };

		public override bool IsEnabledAndSupported( PostProcessRenderContext context )
		{
			return enabled.value;
		}
	}

	public sealed class PixelateRenderer : PostProcessEffectRenderer<Pixelate>
	{
		public override void Render( PostProcessRenderContext context )
		{
			var sheet = context.propertySheets.Get( Shader.Find( "Hidden/Xam/Pixelate" ) );

			sheet.properties.SetVector( "_ScreenResolution", settings.ScreenResolution );
			sheet.properties.SetVector( "_CellSize", settings.CellSize );

			context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
		}
	}
}