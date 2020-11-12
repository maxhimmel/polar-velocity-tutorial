using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Xam.Cinemachine
{
	public class PathLineRenderer : MonoBehaviour
	{
		private LineRenderer m_lineRenderer = null;
		private CinemachinePathBase m_path = null;

		private void Start()
		{
			m_path.PopulateLineRenderPoints( m_lineRenderer );
		}

		private void Awake()
		{
			m_path = GetComponentInParent<CinemachinePathBase>();
			m_lineRenderer = GetComponentInChildren<LineRenderer>();
		}
	}
}