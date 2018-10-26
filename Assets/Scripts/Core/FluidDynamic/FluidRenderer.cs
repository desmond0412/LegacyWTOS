﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artoncode.Core.Fluid {
	[RequireComponent(typeof(ParticleRenderer))]
	[RequireComponent(typeof(ParticleEmitter))]
	public class FluidRenderer : MonoBehaviour {

		public FluidSystem fluidSystem;
		public float particleDrawScale = 4f;
		private float radius;

		// Use this for initialization
		void Start () {
			radius = fluidSystem.fluidSystemDef.radius;
		}
	
		// Update is called once per frame
		void FixedUpdate () {
			int c = fluidSystem.getParticleCount () - GetComponent<ParticleEmitter>().particleCount;
			GetComponent<ParticleEmitter>().Emit (c);
			Particle[] particles = GetComponent<ParticleEmitter>().particles;
			List<FluidParticle> fps = fluidSystem.getParticles ();
			for (int i=0; i<particles.Length; i++) {
				particles [i].position = fps [i].position;
				//float w = 1-Mathf.Clamp(fps[i].weight, 0.0f, 0.8f);

				particles [i].color = fps[i].temperature<fps[i].boilingTemperature ? fps[i].color: new Color(1f,1f,1f,0.3f);//fps [i].color;//new Color(fps[i].color.r, fps[i].color.g, fps[i].color.b, w);
				if (fps[i].temperature>=fps[i].boilingTemperature)
					fps[i].mass = -Mathf.Abs(fps[i].mass);
				else
					fps[i].mass = Mathf.Abs(fps[i].mass);
				particles [i].size = radius * particleDrawScale;
				particles [i].energy = 1;
				particles [i].velocity = fps [i].velocity;
			}
			GetComponent<ParticleEmitter>().particles = particles;
		}
	}
}