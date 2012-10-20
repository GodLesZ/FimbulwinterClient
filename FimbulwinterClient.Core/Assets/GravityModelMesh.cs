﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FimbulwinterClient.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FimbulwinterClient.Core.Assets
{
    public class GravityModelMesh
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private string _parentName;
        public string ParentName
        {
            get { return _parentName; }
        }

        private Texture2D[] _textures;
        public Texture2D[] Textures
        {
            get { return _textures; }
        }

        private Matrix _matrix;
        public Matrix Matrix
        {
            get { return _matrix; }
        }

        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
        }

        private Vector3 _position2;
        public Vector3 Position2
        {
            get { return _position2; }
        }

        private VertexBuffer _vertices;
        public VertexBuffer Vertices
        {
            get { return _vertices; }
        }

        private IndexBuffer[] _indexes;
        public IndexBuffer[] Indexes
        {
            get { return _indexes; }
        }

        private Tuple<Quaternion, int>[] _rotationFrames;
        public Tuple<Quaternion, int>[] RotationFrames
        {
            get { return _rotationFrames; }
        }

        private float _rotationAngle;
        public float RotationAngle
        {
            get { return _rotationAngle; }
        }

        private Vector3 _rotationAxis;
        public Vector3 RotationAxis
        {
            get { return _rotationAxis; }
        }

        private Vector3 _scale;
        public Vector3 Scale
        {
            get { return _scale; }
        }

        private GravityModelMesh _parent;
        public GravityModelMesh Parent
        {
            get { return _parent; }
        }

        private GravityModelMesh[] _children;
        public GravityModelMesh[] Children
        {
            get { return _children; }
        }

        public GravityModelMesh()
        {
            _children = new GravityModelMesh[0];
        }

        public void Load(GravityModel owner, BinaryReader br, byte majorVersion, byte minorVersion)
        {
            _name = br.ReadCString(40);
            _parentName = br.ReadCString(40);

            _textures = new Texture2D[br.ReadInt32()];
            for (int i = 0; i < _textures.Length; i++)
            {
                _textures[i] = owner.Textures[br.ReadInt32()];
            }

            _matrix = Matrix.Identity;

            _matrix.M11 = br.ReadSingle();
            _matrix.M12 = br.ReadSingle();
            _matrix.M13 = br.ReadSingle();

            _matrix.M21 = br.ReadSingle();
            _matrix.M22 = br.ReadSingle();
            _matrix.M23 = br.ReadSingle();

            _matrix.M31 = br.ReadSingle();
            _matrix.M32 = br.ReadSingle();
            _matrix.M33 = br.ReadSingle();

            _position.X = br.ReadSingle();
            _position.Y = br.ReadSingle();
            _position.Z = br.ReadSingle();

            _position2.X = br.ReadSingle();
            _position2.Y = br.ReadSingle();
            _position2.Z = br.ReadSingle();

            _rotationAngle = br.ReadSingle();

            _rotationAxis.X = br.ReadSingle();
            _rotationAxis.Y = br.ReadSingle();
            _rotationAxis.Z = br.ReadSingle();

            _scale.X = br.ReadSingle();
            _scale.Y = br.ReadSingle();
            _scale.Z = br.ReadSingle();

            Vector3[] vertices = new Vector3[br.ReadInt32()];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].X = br.ReadSingle();
                vertices[i].Y = br.ReadSingle();
                vertices[i].Z = br.ReadSingle();
            }

            Vector2[] texcoords = new Vector2[br.ReadInt32()];
            for (int i = 0; i < texcoords.Length; i++)
            {
                if (majorVersion > 1 || (majorVersion == 1 && minorVersion >= 2))
                    br.ReadSingle();

                texcoords[i].X = br.ReadSingle();
                texcoords[i].Y = br.ReadSingle();
            }

            List<VertexPositionNormalTexture> ggvertices = new List<VertexPositionNormalTexture>();
            List<int>[] indexes = new List<int>[_textures.Length];

            for (int i = 0; i < _textures.Length; i++)
                indexes[i] = new List<int>();

            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int start = i * 3;

                Vector3 v1 = vertices[br.ReadInt16()];
                Vector3 v2 = vertices[br.ReadInt16()];
                Vector3 v3 = vertices[br.ReadInt16()];

                Vector2 t1 = texcoords[br.ReadInt16()];
                Vector2 t2 = texcoords[br.ReadInt16()];
                Vector2 t3 = texcoords[br.ReadInt16()];

                int tex = br.ReadInt16();
                br.ReadInt16();
                int twoSide = br.ReadInt32();
                int smoothGroup = br.ReadInt32();

                Vector3 normal = Vector3.Cross(v1 - v3, v2 - v3);
                normal.Normalize();

                ggvertices.Add(new VertexPositionNormalTexture(v1, normal, t1));
                ggvertices.Add(new VertexPositionNormalTexture(v2, normal, t2));
                ggvertices.Add(new VertexPositionNormalTexture(v3, normal, t3));

                indexes[tex].Add(start + 0);
                indexes[tex].Add(start + 1);
                indexes[tex].Add(start + 2);
            }

            gvertices = ggvertices.ToArray();

            _rotationFrames = new Tuple<Quaternion, int>[br.ReadInt32()];
            for (int i = 0; i < _rotationFrames.Length; i++)
            {
                int time = br.ReadInt32();
                Quaternion q = new Quaternion();

                q.X = br.ReadSingle();
                q.Y = br.ReadSingle();
                q.Z = br.ReadSingle();
                q.W = br.ReadSingle();

                _rotationFrames[i] = new Tuple<Quaternion, int>(q, time);
            }
            
            _vertices = new VertexBuffer(SharedInformation.GraphicsDevice, typeof(VertexPositionNormalTexture), gvertices.Length, BufferUsage.WriteOnly);
            _vertices.SetData(gvertices);

            _indexes = new IndexBuffer[_textures.Length];
            for (int i = 0; i < _textures.Length; i++)
            {
                if (indexes[i].Count > 0)
                {
                    _indexes[i] = new IndexBuffer(SharedInformation.GraphicsDevice, typeof(int), indexes[i].Count, BufferUsage.WriteOnly);
                    _indexes[i].SetData(indexes[i].ToArray());
                }
            }
        }

        public void Draw(Matrix world, Effect effect, GameTime gameTime)
        {
            Matrix m = world * GetGlobalMatrix(false, gameTime);

            effect.Parameters["ModelWorld"].SetValue(m * GetLocalMatrix());

            SharedInformation.GraphicsDevice.SetVertexBuffer(_vertices);

            for (int i = 0; i < _textures.Length; i++)
            {
                effect.Parameters["Texture"].SetValue(_textures[i]);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                }

                SharedInformation.GraphicsDevice.Indices = _indexes[i];
                SharedInformation.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.VertexCount, 0, _indexes[i].IndexCount / 3);
            }

            for (int i = 0; i < _children.Length; i++)
            {
                _children[i].Draw(m, effect, gameTime);
            }
        }

        public static Matrix CreateRotationAxis(Vector3 vec, float angle)
        {
            float c = (float)System.Math.Cos(angle);
            float s = (float)System.Math.Sin(angle);
            float t = 1 - c;
            Matrix m =
                    new Matrix(t * vec.X * vec.X + c, t * vec.X * vec.Y + s * vec.Z, t * vec.X * vec.Z + s * vec.Y, 0,
                                 t * vec.X * vec.Y - s * vec.Z, t * vec.Y * vec.Y + c, t * vec.Y * vec.Z + s * vec.X, 0,
                                 t * vec.X * vec.Z + s * vec.Y, t * vec.Y * vec.Z - s * vec.X, t * vec.Z * vec.Z + c, 0,
                                 0, 0, 0, 1);
            return Matrix.Transpose(m);
        }

        private Matrix _globalMatrix;
        private bool _globalMatrixCached;
        private double _lastTick;
        private Matrix GetGlobalMatrix(bool animated, GameTime gameTime)
        {
            if (_globalMatrixCached)
                return _globalMatrix;
            
            _globalMatrix = Matrix.Identity;

            if (_parent == null)
            {
                if (_children.Length > 0)
                _localMatrix *= Matrix.CreateTranslation(-bbrange.X, -bbmax.Y, -bbrange.Z);
                else
                _localMatrix *= Matrix.CreateTranslation(0, -bbmax.Y + bbrange.Y, 0);
            }
            else
            {
                _globalMatrix *= Matrix.CreateTranslation(_position);
            }

            if (_rotationFrames.Length == 0)
            {
                _globalMatrix *= CreateRotationAxis(_rotationAxis, _rotationAngle);
            }
            else
            {
                if (animated)
                {
                    int current = 0;

                    for (int i = 0; i < _rotationFrames.Length; i++)
                    {
                        if (_rotationFrames[i].Item2 > _lastTick)
                        {
                            current = i - 1;
                            break;
                        }
                    }

                    if (current < 0)
                        current = 0;

                    int next = current + 1;
                    if (next >= _rotationFrames.Length)
                        next = 0;

                    float interval = ((float)(_lastTick - _rotationFrames[current].Item2)) / ((float) (_rotationFrames[next].Item2 - _rotationFrames[current].Item2));
                    Quaternion q = Quaternion.Lerp(_rotationFrames[current].Item1, _rotationFrames[next].Item1, interval);
                    q.Normalize();

                    _globalMatrix *= Matrix.CreateFromQuaternion(q);

                    _lastTick += gameTime.ElapsedGameTime.TotalMilliseconds;
                    while (_lastTick > _rotationFrames[_rotationFrames.Length - 1].Item2)
                        _lastTick -= _rotationFrames[_rotationFrames.Length - 1].Item2;
                }
                else
                {
                    Quaternion q = _rotationFrames[0].Item1;

                    q.Normalize();

                    _globalMatrix *= Matrix.CreateFromQuaternion(q);
                }
            }
            
            _globalMatrix *= Matrix.CreateScale(_scale);
            
            if (_rotationFrames.Length == 0)
                _globalMatrixCached = true;

            return _globalMatrix;
        }

        private Matrix _localMatrix;
        private bool _localMatrixCached;
        private Matrix GetLocalMatrix()
        {
            if (_localMatrixCached)
                return _localMatrix;

            _localMatrix = Matrix.Identity;

            if (_parent == null && _children.Length > 0)
                _localMatrix *= Matrix.CreateTranslation(-bbrange.X, -bbrange.Y, -bbrange.Z);

            if (Parent != null || _children.Length == 0)
                _localMatrix *= Matrix.CreateTranslation(_position.X, _position.Y, _position.Z);

            _localMatrix *= _matrix; 
            _localMatrixCached = true;

            return _localMatrix;
        }

        public void SetBoundingBox(ref Vector3 _bbmin, ref Vector3 _bbmax)
        {
	        int i;

	        bbmin = new Vector3(9999999, 9999999, 9999999);
	        bbmax = new Vector3(-9999999, -9999999, -9999999);

	        if(_parent != null)
		        bbmin = bbmax = new Vector3(0, 0, 0);

	        Matrix myMat = _matrix;
            for (i = 0; i < gvertices.Length / 3; i++)
	        {
		        for(int ii = 0; ii < 3; ii++)
		        {
                    Vector3 v = Vector3.Transform(gvertices[i + ii].Position, myMat);

			        if(_parent != null || _children.Length > 0)
				        v += _position + _position2;

                    bbmin.X = Math.Min(bbmin.X, v.X);
                    bbmin.Y = Math.Min(bbmin.Y, v.Y);
                    bbmin.Z = Math.Min(bbmin.Z, v.Z);

                    bbmax.X = Math.Max(bbmax.X, v.X);
                    bbmax.Y = Math.Max(bbmax.Y, v.Y);
                    bbmax.Z = Math.Max(bbmax.Z, v.Z);
		        }
	        }

	        bbrange = (_bbmin + _bbmax) / 2.0f;

            _bbmin.X = Math.Min(_bbmin.X, bbmin.X);
            _bbmin.Y = Math.Min(_bbmin.Y, bbmin.Y);
            _bbmin.Z = Math.Min(_bbmin.Z, bbmin.Z);

            _bbmax.X = Math.Max(_bbmax.X, bbmax.X);
            _bbmax.Y = Math.Max(_bbmax.Y, bbmax.Y);
            _bbmax.Z = Math.Max(_bbmax.Z, bbmax.Z);
            
            for (i = 0; i < _children.Length; i++)
            {
		        _children[i].SetBoundingBox(ref _bbmin, ref _bbmax);
            }
        }

        public void SetBoundingBox2(Matrix mat, ref Vector3 _bbmin, ref Vector3 _bbmax)
        {
	        Matrix myMat = mat * GetGlobalMatrix(false, null);
            Matrix mat2 = myMat * GetLocalMatrix();
	        int i;

            for (i = 0; i < gvertices.Length / 3; i++)
	        {
		        for(int ii = 0; ii < 3; ii++)
		        {
                    Vector3 v = Vector3.Transform(gvertices[i + ii].Position, mat2);
        
                    bbmin.X = Math.Min(bbmin.X, v.X);
                    bbmin.Y = Math.Min(bbmin.Y, v.Y);
                    bbmin.Z = Math.Min(bbmin.Z, v.Z);

                    bbmax.X = Math.Max(bbmax.X, v.X);
                    bbmax.Y = Math.Max(bbmax.Y, v.Y);
                    bbmax.Z = Math.Max(bbmax.Z, v.Z);
		        }
	        }

	        bbrange = (_bbmin + _bbmax) / 2.0f;

            _bbmin.X = Math.Min(_bbmin.X, bbmin.X);
            _bbmin.Y = Math.Min(_bbmin.Y, bbmin.Y);
            _bbmin.Z = Math.Min(_bbmin.Z, bbmin.Z);

            _bbmax.X = Math.Max(_bbmax.X, bbmax.X);
            _bbmax.Y = Math.Max(_bbmax.Y, bbmax.Y);
            _bbmax.Z = Math.Max(_bbmax.Z, bbmax.Z);
	
            for (i = 0; i < _children.Length; i++)
            {
		        _children[i].SetBoundingBox2(mat, ref _bbmin, ref _bbmax);
            }	
        }

        Vector3 bbmin, bbmax, bbrange;
        VertexPositionNormalTexture[] gvertices;

        public void CreateChildren(GravityModelMesh[] _meshes)
        {
            List<GravityModelMesh> childs = new List<GravityModelMesh>();

            foreach (GravityModelMesh m in _meshes)
            {
                if (string.Compare(m.ParentName, _name, true) == 0)
                    childs.Add(m);
            }

            foreach (GravityModelMesh m in _meshes)
            {
                if (string.Compare(m.Name, _parentName, true) == 0)
                {
                    _parent = m;
                    break;
                }
            }

            _children = childs.ToArray();
        }
    }
}
