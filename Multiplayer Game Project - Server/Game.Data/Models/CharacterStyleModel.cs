using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Abstracts;
using Network.Data;
using Network.Data.Interfaces;

namespace Game.Data.Models
{
    public class CharacterStyleModel : APacketWrapper, IModel
    {
        public int ID { get; set; }

        public short Hair { get; set; }

        public float CraniumSlope { get; set; }
        public float EarlobesAttached { get; set; }
        public float EarsDepth { get; set; }
        public float EarsElf { get; set; }
        public float EarsHeight { get; set; }
        public float JawOutIn { get; set; }
        public float LipBottomInOut { get; set; }
        public float LipBottomInOutL { get; set; }
        public float LipBottomInOutR { get; set; }
        public float LipBottomUpDown { get; set; }
        public float LipBottomUpDownL { get; set; }
        public float LipBottomUpDownR { get; set; }
        public float JawCurve { get; set; }
        public float JawHeight { get; set; }
        public float JawlineDepth { get; set; }
        public float lHornFrontHigh { get; set; }
        public float lHornFrontLarge { get; set; }
        public float lHornFrontSmall { get; set; }
        public float lHornSide { get; set; }
        public float rHornFrontHigh { get; set; }
        public float rHornFrontLarge { get; set; }
        public float rHornFrontSmall { get; set; }
        public float rHornSide { get; set; }
        public float EyelashesLength { get; set; }
        public float EyelidSize { get; set; }
        public float EyelidsFoldDown { get; set; }
        public float EyesAlmondInner { get; set; }
        public float EyesAlmondOuter { get; set; }
        public float EyesAngledInner { get; set; }
        public float EyesAngledOuter { get; set; }
        public float EyesHeight { get; set; }
        public float EyesIrisSize { get; set; }
        public float EyesSize { get; set; }
        public float EyesWidth { get; set; }
        public float EyelidsLowerUpDown { get; set; }
        public float EyelidsLowerUpDownL { get; set; }
        public float EyelidsLowerUpDownR { get; set; }
        public float EyelidsUpperDownUp { get; set; }
        public float EyelidsUpperDownUpL { get; set; }
        public float EyelidsUpperDownUpR { get; set; }
        public float EyesSquint { get; set; }
        public float EyesSquintL { get; set; }
        public float EyesSquintR { get; set; }
        public float EyeSagTop { get; set; }
        public float FaceCenterDepth { get; set; }
        public float FaceFlat { get; set; }
        public float FaceHeart { get; set; }
        public float FaceRound { get; set; }
        public float FaceSquare { get; set; }
        public float BrowInnerUpDown { get; set; }
        public float BrowInnerUpDownL { get; set; }
        public float BrowInnerUpDownR { get; set; }
        public float BrowOuterUpDown { get; set; }
        public float BrowOuterUpDownL { get; set; }
        public float BrowOuterUpDownR { get; set; }
        public float BrowUpDown { get; set; }
        public float BrowUpDownL { get; set; }
        public float BrowUpDownR { get; set; }
        public float CheekbonesDepressionHD { get; set; }
        public float CheeksDepth { get; set; }
        public float ChinCleftHD { get; set; }
        public float ChinWidth { get; set; }
        public float LipsPart { get; set; }
        public float LipsPartCenter { get; set; }
        public float LipsPucker { get; set; }
        public float LipTopUpDown { get; set; }
        public float LipTopUpDownL { get; set; }
        public float LipTopUpDownR { get; set; }
        public float MouthCornerUpDown { get; set; }
        public float MouthFrown { get; set; }
        public float MouthNarrow { get; set; }
        public float MouthNarrowL { get; set; }
        public float MouthNarrowR { get; set; }
        public float MouthCurves { get; set; }
        public float MouthHeight { get; set; }
        public float MouthSize { get; set; }
        public float MouthWidth { get; set; }
        public float LipDepthLower { get; set; }
        public float LipsHeart { get; set; }
        public float LipsSquare { get; set; }
        public float LipUpperSize { get; set; }
        public float NoseAge { get; set; }
        public float NoseBridgeDepth { get; set; }
        public float NoseBridgeHeight { get; set; }
        public float NoseBridgeSkew { get; set; }
        public float NoseBridgeSlope { get; set; }
        public float NoseBump { get; set; }
        public float NoseDepth { get; set; }
        public float NoseHeight { get; set; }
        public float NosePinch { get; set; }
        public float NoseRidgeWidth { get; set; }
        public float NoseSize { get; set; }
        public float NoseTipHeight { get; set; }
        public float NoseTipRound { get; set; }
        public float NoseWidth { get; set; }
        public float NostrilWingSize { get; set; }
        public float NostrilWingWidth { get; set; }
        public float UpperArmsSize { get; set; }
        public float ArmsLength { get; set; }
        public float FingersLength { get; set; }
        public float FingersLengthL { get; set; }
        public float FingersLengthR { get; set; }
        public float FingersWidth { get; set; }
        public float FingersWidthL { get; set; }
        public float FingersWidthR { get; set; }
        public float ForearmsSize { get; set; }
        public float PalmScale { get; set; }
        public float PalmScaleL { get; set; }
        public float PalmScaleR { get; set; }
        public float BasicWeight1 { get; set; }
        public float BodybuilderSize { get; set; }
        public float BodybuilderDetails { get; set; }
        public float FitnessSize { get; set; }
        public float FitnessDetails { get; set; }
        public float NeckWeight { get; set; }
        public float NeckLength { get; set; }
        public float NeckSize { get; set; }
        public float ChestDepth { get; set; }
        public float ChestWidth { get; set; }
        public float ShldrsScale { get; set; }
        public float ShldrsWidth { get; set; }
        public float ShoulderDrop { get; set; }
        public float ShouldersSize { get; set; }
        public float PectoralsCleavage { get; set; }
        public float PectoralsDiameter { get; set; }
        public float PectoralsHeavy { get; set; }
        public float PectoralsSag { get; set; }
        public float RibcageSize { get; set; }
        public float SternumDepth { get; set; }
        public float Belly { get; set; }
        public float StomachLowerDepth { get; set; }
        public float ThighsSize { get; set; }
        public float CalvesSize { get; set; }
        public float GlutesSize { get; set; }
        public float LegsLength { get; set; }
        public float ShinsSize { get; set; }
    }
}