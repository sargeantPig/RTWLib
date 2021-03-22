using RTWLib.Functions.DMB.ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB
{
    public class BattleModel : IdmbOutput
    {
        public ModelType modelType;
        public Skeletons skeletons;
        public ModelScale modelScale;
        public ModelIRange modelIRange;
        public List<Texture> texture;
        public List<ModelFlexi> modelFlexi;
        public List<ModelSprite> modelSprite;
        public ModelTri modelTri;

        public BattleModel()
        {
            modelType = new ModelType();
            skeletons = new Skeletons();
            modelScale = new ModelScale();
            modelIRange = new ModelIRange();
            texture = new List<Texture>();
            modelFlexi = new List<ModelFlexi>();
            modelSprite = new List<ModelSprite>();
            modelTri = new ModelTri();
        }

        public void AddMercLine()
        {
            bool hasMerc = false;

            if (texture == null || texture.Count == 0)
                return;

            foreach (var t in texture)
            {
                if (t.HasMercLine())
                {
                    hasMerc = true;
                    break;
                }
            }

            if (!hasMerc)
                texture.Add(new Texture("merc", texture[new Random().Next(0, texture.Count())].filepath));
        }

        public string Output()
        {
            string str = "";

            str = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                modelType.Output(),
                skeletons.Output(),
                modelScale.Output(),
                modelIRange.Output(),
                DMB.IdmbArrayToString(texture.ToArray()),
                DMB.IdmbArrayToString(modelFlexi.ToArray()),
                DMB.IdmbArrayToString(modelSprite.ToArray()),
                modelTri.Output());

            return str.CarriageReturnNewLine();
        }
    }
}
