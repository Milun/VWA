using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Particle
    {
        private Billboard bill;

        private Vector2 scale;
        private Vector3 pos = Vector3.Zero;
        private Vector3 vel;
        private float rot;
        private float rot2 = 0.0f;
        private float rot2Speed = 0.0f;

        private float time = 0.0f;
        private float timeUV = 0.0f;

        private float lifetime;

        List<Tuple<float, float>> keyRot;
        List<Tuple<float, float>> keyAlpha;
        List<Tuple<float, Vector3>> keyVel;
        List<Tuple<float, Vector2>> keyScale;
        List<Tuple<float, Vector2>> keyUV;

        int curRot, curAlpha, curVel, curScale, curUV = 0;


        public Particle(Texture2D tex,
                        Vector3 _pos,
                        Vector2 size,
                        Vector2 origin,
                        Vector2 offPos,
                        Vector2 offSize,
                        float _lifetime)
        {
            pos = _pos;

            bill = new Billboard(tex, pos, origin, size, offPos, offSize);

            keyRot = new List<Tuple<float, float>>();
            keyAlpha = new List<Tuple<float, float>>();
            keyVel = new List<Tuple<float, Vector3>>();
            keyScale = new List<Tuple<float, Vector2>>();
            keyUV = new List<Tuple<float, Vector2>>();

            vel = Vector3.Zero;
            rot = 0;
            scale = size;

            keyRot.Add(     new Tuple<float, float>(0f, rot));
            keyAlpha.Add(   new Tuple<float, float>(0f, 1f));
            keyVel.Add(     new Tuple<float, Vector3>(0f, vel));
            keyScale.Add(   new Tuple<float, Vector2>(0f, Vector2.Zero));
            keyUV.Add(      new Tuple<float, Vector2>(0f, offPos));

            lifetime = _lifetime;
        }

        public void addKeyRot(float t, float r)
        {
            Tuple<float, float> tuple = new Tuple<float, float>(t, r);

            if (keyRot.Last().Item1 == t)
            {
                keyRot[keyRot.Count - 1] = tuple;

            }
            else
            {
                keyRot.Add(tuple);
            }

            if (t == 0f)
            {
                rot = r;
                bill.SetRotation(r);
            }
        }

        public void setRot2(float r)
        {
            rot2Speed = r;
        }

        public void addKeyAlpha(float t, float a)
        {
            Tuple<float, float> tuple = new Tuple<float, float>(t, a);

            if (keyAlpha.Last().Item1 == t)
            {
                keyAlpha[keyAlpha.Count - 1] = tuple;

            }
            else
            {
                keyAlpha.Add(tuple);
            }

            if (t == 0f)
            {
                bill.SetAlpha(a);
            }
        }

        public void addKeyVel(float t, Vector3 v)
        {
            Tuple<float, Vector3> tuple = new Tuple<float, Vector3>(t, v);

            if (keyVel.Last().Item1 == t)
            {
                keyVel[keyVel.Count-1] = tuple;
                
            }
            else
            {
                keyVel.Add(tuple);
            }

            if (t == 0f)
            {
                vel = v;

                // Reworking this...
                //bill.UpdatePos(vel);
            }
        }

        public void addKeyScale(float t, Vector2 s)
        {
            Tuple<float, Vector2> tuple = new Tuple<float, Vector2>(t, s);

            if (keyScale.Last().Item1 == t)
            {
                keyScale[keyScale.Count - 1] = tuple;

            }
            else
            {
                keyScale.Add(tuple);
            }

            if (t == 0f)
            {
                scale = s;
            }
        }

        public void addKeyUV(float t, Vector2 uv)
        {
            Tuple<float, Vector2> tuple = new Tuple<float, Vector2>(t, uv);

            if (keyUV.Last().Item1 == t)
            {
                keyUV[keyUV.Count - 1] = tuple;

            }
            else
            {
                keyUV.Add(tuple);
            }
        }

        public void Draw()
        {
            // As long as the particle is alive, animate it.
            /*if (time < lifetime)
            {
                time += 0.1f;
                rotation += rotate;
                pos += move;

                bill.SetRotation(rotation);
                bill.SetAlpha(1f - (time / lifetime));
                bill.UpdatePos(pos);

                bill.Draw();
            }*/

            if (time > lifetime)
            {
                return;
            }

            time += 0.1f;
            timeUV += 0.1f;

            if (keyVel.Count > 1)
            {
                // Check what keyframe we're on.
                if (curVel+1 < keyVel.Count)
                {
                    if (keyVel[curVel].Item1 <= time &&
                        keyVel[curVel + 1].Item1 > time)
                    {
                        // Get percent.
                        float percent = (time - keyVel[curVel].Item1) /
                                        (keyVel[curVel + 1].Item1 - keyVel[curVel].Item1);

                        vel = keyVel[curVel].Item2 + (keyVel[curVel + 1].Item2 - keyVel[curVel].Item2) * percent;

                        pos += vel;
                        bill.UpdatePos(pos);
                    }
                    else if (keyVel[curVel + 1].Item1 <= time)
                    {
                        curVel++;
                    }
                }
            }

            if (keyScale.Count > 1)
            {
                // Check what keyframe we're on.
                if (curScale + 1 < keyScale.Count)
                {
                    if (keyScale[curScale].Item1 <= time &&
                        keyScale[curScale + 1].Item1 > time)
                    {
                        // Get percent.
                        float percent = (time - keyScale[curScale].Item1) /
                                        (keyScale[curScale + 1].Item1 - keyScale[curScale].Item1);

                        scale = keyScale[curScale].Item2 + (keyScale[curScale + 1].Item2 - keyScale[curScale].Item2) * percent;
                    }
                    else if (keyScale[curScale + 1].Item1 <= time)
                    {
                        curScale++;
                    }
                }
            }

            // The last frame never counts. It just tells it when to loop.
            if (keyUV.Count > 1)
            {
                // Check what keyframe we're on.
                if (curUV+2 < keyUV.Count)
                {
                    if (keyUV[curUV+1].Item1 < timeUV)
                    {
                        curUV++;
                    }
                }
                else
                {
                    curUV = 0;
                    timeUV = 0.0f;
                }
                
                bill.SetUV(keyUV[curUV].Item2);
            }


            if (keyAlpha.Count > 1)
            {
                // Check what keyframe we're on.
                if (curAlpha + 1 < keyAlpha.Count)
                {
                    if (keyAlpha[curAlpha].Item1 <= time &&
                        keyAlpha[curAlpha + 1].Item1 > time)
                    {
                        // Get percent.
                        float percent = (time - keyAlpha[curAlpha].Item1) /
                                        (keyAlpha[curAlpha + 1].Item1 - keyAlpha[curAlpha].Item1);

                        float alpha = keyAlpha[curAlpha].Item2 + (keyAlpha[curAlpha + 1].Item2 - keyAlpha[curAlpha].Item2) * percent;

                        bill.SetAlpha(alpha);
                    }
                    else if (keyAlpha[curAlpha + 1].Item1 <= time)
                    {
                        curAlpha++;
                    }
                }
            }

            if (keyRot.Count > 1)
            {
                // Check what keyframe we're on.
                if (curRot + 1 < keyRot.Count)
                {
                    if (keyRot[curRot].Item1 <= time &&
                        keyRot[curRot + 1].Item1 > time)
                    {
                        // Get percent.
                        float percent = (time - keyRot[curRot].Item1) /
                                        (keyRot[curRot + 1].Item1 - keyRot[curRot].Item1);

                        rot = keyRot[curRot].Item2 + (keyRot[curRot + 1].Item2 - keyRot[curRot].Item2) * percent;
                    }
                    else if (keyRot[curRot + 1].Item1 <= time)
                    {
                        curRot++;
                    }
                }
            }

            rot2 += rot2Speed;

            if (rot2Speed != 0.0f || keyRot.Count > 1)
            {
                bill.SetRotation(rot, rot2);
            }

            bill.UpdateVerts(scale.X, scale.Y);
            bill.Draw();
        }
    }
}
