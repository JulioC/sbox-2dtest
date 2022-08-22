﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.UI;

namespace Sandbox
{
	public partial class Sprite : ModelEntity
	{
		private Material _material;
		private bool _materialInvalid;
		private Texture _texture;

		private float _localRotation;
		private bool _firstFrame = true;

		[Net, Change]
		public string TexturePath { get; set; }

		[Net]
		public new Vector2 Scale { get; set; }

		public new Vector2 Position
		{
			get => base.Position;
			set => base.Position = new Vector3( value.x, value.y, base.Position.z );
		}

		public new Vector2 LocalPosition
        {
			get => base.LocalPosition;
			set => base.LocalPosition = new Vector3(value.x, value.y, base.LocalPosition.z);
		}

		public float Depth
		{
			get => base.Position.z;
			set => base.Position = base.Position.WithZ( value );
		}

		public new float Rotation
		{
			get => base.Rotation.Angles().yaw + 90f;
			set
			{
				base.Rotation = global::Rotation.FromYaw( value - 90f );
			}
		}

		public new float LocalRotation
		{
			get => _localRotation;
			set
			{
				_localRotation = value;
				base.LocalRotation = global::Rotation.FromYaw(LocalRotation - 90f);
			}
		}

		internal Material Material
		{
			get => _material ??= Material.Load( "materials/test_sprite.vmat" ).CreateCopy();
		}

		private void OnTexturePathChanged()
		{
			_texture = string.IsNullOrEmpty( TexturePath )
				? Texture.White
				: Texture.Load( FileSystem.Mounted, TexturePath );

			Material.OverrideTexture( "g_tColor", _texture );
			_materialInvalid = true;

		}
		
		public Sprite()
		{

		}

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/quad.vmdl" );

			EnableDrawing = true;

			Rotation = 0f;
			Scale = new Vector2( 1f, 1f );
		}
		
		[Event.PreRender]
		private void ClientPreRender()
		{
			SceneObject.Flags.IsTranslucent = true;
			SceneObject.Attributes.Set( "SpriteScale", new Vector2(Scale.y, Scale.x) / 100f );

			if ( _material != null && _materialInvalid )
			{
				SetMaterialOverride( _material );
			}
		}
	}
}
