using RTWLib.Data;
using RTWLib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RTWLib.Objects.Descr_strat;

namespace RTWLib.Medieval2
{
    public class M2ModelBattle : FileBase, IFile
    {
        readonly string merc_line = "4 merc \r\n";
        readonly string unitModelEnd = "16 -0.090000004 0 0 -0.34999999 0.80000001 0.60000002";
        readonly string mountModelEnd = "0 0 0 0 -1 0 0 0 0 0 0 0 0";
        readonly string newLine = "\r\n";
        string data = string.Empty; //store each line
        string tempBlockTextures = string.Empty; // temp block of line to check for merc
        string tempBlockAttachments = string.Empty;
        List<string> tempTextures = new List<string>();
        List<string> tempAttachments = new List<string>();
        Random rand = new Random();
        List<string> modelData = new List<string>();


        public M2ModelBattle() : base(FileNames.battle_models, @"data/unit_models/battle_models.modeldb", "Handles the games battle models")
        { }

        public override void Parse(string[] path, out int lineNumber, out string currentLine)
        {
            StreamReader sr = new StreamReader(path[0]);
            string line;

            lineNumber = 0;
            currentLine = "";
            bool beginUnits = false;
            

            while ((line = sr.ReadLine()) != null)
            {
                //start using only texture parse mounts have no attachments

                //mailed_knights_ug1
                if (!line.Contains("mount_pony") && !beginUnits) // catch first line
                {
                    data += line + newLine;
                }

                else if (line.Contains("mount_pony")) //begin parsing 
                {
                    beginUnits = true;
                }

                if (beginUnits)
                {
                    int texturesInARow = 0;
                    //new system
                    string tempModelData = string.Empty;
                    string tempTextureBlock = string.Empty;
                    string tempLodBlock = string.Empty;
                    string tempAttachmentBlock = string.Empty;
                    int tempFactionCount = 0;
                    List<string> tempTextureStore = new List<string>();
                    List<string> tempAttachmentStore = new List<string>();
                    int fCountFormat = 0;

                    int returnCode = -2;
                    tempLodBlock = LodParse(sr, line, out line);
                    tempFactionCount = FactionCountParse(line, out fCountFormat);
                    tempTextureBlock = TextureBlockParse(sr, line, out line, out tempTextureStore, out texturesInARow,
                        out returnCode, in tempFactionCount);

                    switch (returnCode)
                    {
                        case 1:
                            tempAttachmentBlock = AttachmentBlockParse(sr, line, out line, out tempAttachmentStore);
                            tempModelData = Combine(tempTextureBlock, tempAttachmentBlock, tempLodBlock,  tempFactionCount, fCountFormat, returnCode,
                                texturesInARow, tempTextureStore, tempAttachmentStore);
                            break;
                        case 2:
                            tempModelData = Combine(tempTextureBlock, tempAttachmentBlock, tempLodBlock, tempFactionCount, fCountFormat, returnCode, 
                                texturesInARow, tempTextureStore, tempAttachmentStore);
                            break;
                        default: //error
                            break;
                    }

                    data += tempModelData;


                }
            }

        }

        string LodParse(StreamReader sr, string line, out string ncLine)
        {
            bool lodStarted = false;
            string lodBlock = string.Empty;
            lodBlock += line + newLine;
            ncLine = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                ncLine = line;
                if (!line.Contains(".mesh"))
                {
                    if (lodStarted)
                        return lodBlock;
                    else lodBlock += line + newLine;
                }
                else
                {
                    lodStarted = true;
                    lodBlock += line + newLine;
                }
            }

            return lodBlock;
        }

        string TextureBlockParse(StreamReader sr, string line, out string ncLine, 
            out List<string> textures, out int concurrentTextures, out int finishCode, in int factionCount)
        {
            string textureBlock = string.Empty;
            bool texturesStarted = false;
            bool stopTextureCount = false;
            List<string> textureStore = new List<string>();
            int concurrentTexts = 0;
            ncLine = string.Empty;
            textures = new List<string>();
            concurrentTextures = 0;
            while ((line = sr.ReadLine()) != null)
            {
                ncLine = line;
                int num;
                if (int.TryParse(line.Trim(), out num))
                {
                    if (num == factionCount)
                    {
                        finishCode = 1;
                        concurrentTextures = concurrentTexts;
                        textures = textureStore;
                        return textureBlock;
                    }
                }

                if (line.Contains(unitModelEnd) || line.Contains(mountModelEnd))
                {
                    //model ended
                    textureBlock += line + newLine;
                    finishCode = 2;
                    concurrentTextures = concurrentTexts;
                    textures = textureStore;
                    return textureBlock;
                }

                if (line.Contains("textures") || line.Contains("sprite"))
                {
                    texturesStarted = true;
                    textureStore.Add(line + "\r\n");
                    textureBlock += line + newLine;
                    if (!stopTextureCount)
                        concurrentTexts++;
                }

                else
                {
                    if (texturesStarted)
                        stopTextureCount = true;
                    textureBlock += line + newLine;
                }
            }

            finishCode = -1;
            return textureBlock;
        }

        string AttachmentBlockParse(StreamReader sr, string line, out string ncLine,
            out List<string> textures)
        {
            string textureBlock = string.Empty;
            List<string> textureStore = new List<string>();
            ncLine = string.Empty;

            textures = new List<string>();


            while ((line = sr.ReadLine()) != null)
            {
                ncLine = line;
                if (line.Contains(unitModelEnd) || line.Contains(mountModelEnd))
                {
                    //model ended
                    textureBlock += line + newLine;
                    textures = textureStore;
                    return textureBlock;
                }

                if (line.Contains(".texture"))
                {
                    textureStore.Add(line + "\r\n");
                    textureBlock += line + newLine;
                }

                else
                    textureBlock += line + newLine;
            }
            return textureBlock;
        }

        public string Combine(string textureBlock, string attachBlock, string lodBlock, int factionCount, int format, int code,
            int concTextures, List<string>textures, List<string>attachments)
        {
            string mdata = string.Empty;
            if (!textureBlock.Contains(" merc "))
            {
                factionCount++;
                string sfactionCount = factionCount.ToString() + " " + newLine;

                if (format > 2)
                    sfactionCount = string.Format("0 0 {0} 0 0 " + newLine, factionCount);

                if(code == 2)
                {
                    string mercTextures = ConstructMercLine(concTextures, false, textures);
                    mdata += lodBlock;
                    mdata += sfactionCount;
                    mdata += mercTextures;
                    mdata += textureBlock;
                }

                else if(code == 1)
                {
                    string mercTextures = ConstructMercLine(concTextures, false, textures);
                    string mercAttachments = ConstructMercLine(concTextures, true, attachments);
                    mdata += lodBlock;
                    mdata += sfactionCount;
                    mdata += mercTextures;
                    mdata += textureBlock;
                    mdata += sfactionCount;
                    mdata += mercAttachments;
                    mdata += attachBlock;
                }
            }

            else
            {
                string sfactionCount = factionCount.ToString() + " " + newLine;

                if (format > 2)
                    sfactionCount = string.Format("0 0 {0} 0 0 " + newLine, factionCount);

                if (code == 2)
                {
                    mdata += lodBlock;
                    mdata += sfactionCount;
                    mdata += textureBlock;
                }

                else if (code == 1)
                {
                    mdata += lodBlock;
                    mdata += sfactionCount;
                    mdata += textureBlock;
                    mdata += sfactionCount;
                    mdata += attachBlock;
                }
            }
            return mdata;
        }
        
        

        int FactionCountParse(string line, out int format)
        {
            string[] countSplit = line.Split(' ');
            format = countSplit.Count();
            int countIndex = 0;
            if (countSplit.Count() > 4)
                countIndex = 2;

            return Convert.ToInt32(countSplit[countIndex].Trim());
        }

        string ConstructMercLine(int concTextures, bool isAttachment, List<string>textures)
        {
            int texturesRand = rand.Next(textures.Count() / 3);

            string mtexture = string.Empty;
            string matexture = string.Empty;

            if (!isAttachment)
            {
                for (int i = 0; i < concTextures; i++)
                    mtexture += textures[(texturesRand * concTextures) + i];
                return merc_line + mtexture;
            }

            else
            {
                for (int i = 0; i < 2; i++)
                    mtexture += textures[(texturesRand * 2) + i];
                return merc_line + mtexture;
            }

        }

        public override string Output()
        {
            return data;
        }

        override public void ToFile(string filepath)
        {
            StreamWriter sw = new StreamWriter(filepath);
            sw.Write(Output());
            sw.Close();
        }
    }
}
