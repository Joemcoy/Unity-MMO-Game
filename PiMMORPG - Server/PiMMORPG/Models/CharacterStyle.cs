using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class CharacterStyle : ModelBase
    {
        public short Hair { get; set; }
        public short HairColor { get; set; }

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

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Hair = packet.ReadShort();
            HairColor = packet.ReadShort();
            CraniumSlope = packet.ReadFloat();
            EarlobesAttached = packet.ReadFloat();
            EarsDepth = packet.ReadFloat();
            EarsElf = packet.ReadFloat();
            EarsHeight = packet.ReadFloat();
            JawOutIn = packet.ReadFloat();
            LipBottomInOut = packet.ReadFloat();
            LipBottomInOutL = packet.ReadFloat();
            LipBottomInOutR = packet.ReadFloat();
            LipBottomUpDown = packet.ReadFloat();
            LipBottomUpDownL = packet.ReadFloat();
            LipBottomUpDownR = packet.ReadFloat();
            JawCurve = packet.ReadFloat();
            JawHeight = packet.ReadFloat();
            JawlineDepth = packet.ReadFloat();
            lHornFrontHigh = packet.ReadFloat();
            lHornFrontLarge = packet.ReadFloat();
            lHornFrontSmall = packet.ReadFloat();
            lHornSide = packet.ReadFloat();
            rHornFrontHigh = packet.ReadFloat();
            rHornFrontLarge = packet.ReadFloat();
            rHornFrontSmall = packet.ReadFloat();
            rHornSide = packet.ReadFloat();
            EyelashesLength = packet.ReadFloat();
            EyelidSize = packet.ReadFloat();
            EyelidsFoldDown = packet.ReadFloat();
            EyesAlmondInner = packet.ReadFloat();
            EyesAlmondOuter = packet.ReadFloat();
            EyesAngledInner = packet.ReadFloat();
            EyesAngledOuter = packet.ReadFloat();
            EyesHeight = packet.ReadFloat();
            EyesIrisSize = packet.ReadFloat();
            EyesSize = packet.ReadFloat();
            EyesWidth = packet.ReadFloat();
            EyelidsLowerUpDown = packet.ReadFloat();
            EyelidsLowerUpDownL = packet.ReadFloat();
            EyelidsLowerUpDownR = packet.ReadFloat();
            EyelidsUpperDownUp = packet.ReadFloat();
            EyelidsUpperDownUpL = packet.ReadFloat();
            EyelidsUpperDownUpR = packet.ReadFloat();
            EyesSquint = packet.ReadFloat();
            EyesSquintL = packet.ReadFloat();
            EyesSquintR = packet.ReadFloat();
            EyeSagTop = packet.ReadFloat();
            FaceCenterDepth = packet.ReadFloat();
            FaceFlat = packet.ReadFloat();
            FaceHeart = packet.ReadFloat();
            FaceRound = packet.ReadFloat();
            FaceSquare = packet.ReadFloat();
            BrowInnerUpDown = packet.ReadFloat();
            BrowInnerUpDownL = packet.ReadFloat();
            BrowInnerUpDownR = packet.ReadFloat();
            BrowOuterUpDown = packet.ReadFloat();
            BrowOuterUpDownL = packet.ReadFloat();
            BrowOuterUpDownR = packet.ReadFloat();
            BrowUpDown = packet.ReadFloat();
            BrowUpDownL = packet.ReadFloat();
            BrowUpDownR = packet.ReadFloat();
            CheekbonesDepressionHD = packet.ReadFloat();
            CheeksDepth = packet.ReadFloat();
            ChinCleftHD = packet.ReadFloat();
            ChinWidth = packet.ReadFloat();
            LipsPart = packet.ReadFloat();
            LipsPartCenter = packet.ReadFloat();
            LipsPucker = packet.ReadFloat();
            LipTopUpDown = packet.ReadFloat();
            LipTopUpDownL = packet.ReadFloat();
            LipTopUpDownR = packet.ReadFloat();
            MouthCornerUpDown = packet.ReadFloat();
            MouthFrown = packet.ReadFloat();
            MouthNarrow = packet.ReadFloat();
            MouthNarrowL = packet.ReadFloat();
            MouthNarrowR = packet.ReadFloat();
            MouthCurves = packet.ReadFloat();
            MouthHeight = packet.ReadFloat();
            MouthSize = packet.ReadFloat();
            MouthWidth = packet.ReadFloat();
            LipDepthLower = packet.ReadFloat();
            LipsHeart = packet.ReadFloat();
            LipsSquare = packet.ReadFloat();
            LipUpperSize = packet.ReadFloat();
            NoseAge = packet.ReadFloat();
            NoseBridgeDepth = packet.ReadFloat();
            NoseBridgeHeight = packet.ReadFloat();
            NoseBridgeSkew = packet.ReadFloat();
            NoseBridgeSlope = packet.ReadFloat();
            NoseBump = packet.ReadFloat();
            NoseDepth = packet.ReadFloat();
            NoseHeight = packet.ReadFloat();
            NosePinch = packet.ReadFloat();
            NoseRidgeWidth = packet.ReadFloat();
            NoseSize = packet.ReadFloat();
            NoseTipHeight = packet.ReadFloat();
            NoseTipRound = packet.ReadFloat();
            NoseWidth = packet.ReadFloat();
            NostrilWingSize = packet.ReadFloat();
            NostrilWingWidth = packet.ReadFloat();
            UpperArmsSize = packet.ReadFloat();
            ArmsLength = packet.ReadFloat();
            FingersLength = packet.ReadFloat();
            FingersLengthL = packet.ReadFloat();
            FingersLengthR = packet.ReadFloat();
            FingersWidth = packet.ReadFloat();
            FingersWidthL = packet.ReadFloat();
            FingersWidthR = packet.ReadFloat();
            ForearmsSize = packet.ReadFloat();
            PalmScale = packet.ReadFloat();
            PalmScaleL = packet.ReadFloat();
            PalmScaleR = packet.ReadFloat();
            BasicWeight1 = packet.ReadFloat();
            BodybuilderSize = packet.ReadFloat();
            BodybuilderDetails = packet.ReadFloat();
            FitnessSize = packet.ReadFloat();
            FitnessDetails = packet.ReadFloat();
            NeckWeight = packet.ReadFloat();
            NeckLength = packet.ReadFloat();
            NeckSize = packet.ReadFloat();
            ChestDepth = packet.ReadFloat();
            ChestWidth = packet.ReadFloat();
            ShldrsScale = packet.ReadFloat();
            ShldrsWidth = packet.ReadFloat();
            ShoulderDrop = packet.ReadFloat();
            ShouldersSize = packet.ReadFloat();
            PectoralsCleavage = packet.ReadFloat();
            PectoralsDiameter = packet.ReadFloat();
            PectoralsHeavy = packet.ReadFloat();
            PectoralsSag = packet.ReadFloat();
            RibcageSize = packet.ReadFloat();
            SternumDepth = packet.ReadFloat();
            Belly = packet.ReadFloat();
            StomachLowerDepth = packet.ReadFloat();
            ThighsSize = packet.ReadFloat();
            CalvesSize = packet.ReadFloat();
            GlutesSize = packet.ReadFloat();
            LegsLength = packet.ReadFloat();
            ShinsSize = packet.ReadFloat();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteShort(Hair);
            packet.WriteShort(HairColor);
            packet.WriteFloat(CraniumSlope);
            packet.WriteFloat(EarlobesAttached);
            packet.WriteFloat(EarsDepth);
            packet.WriteFloat(EarsElf);
            packet.WriteFloat(EarsHeight);
            packet.WriteFloat(JawOutIn);
            packet.WriteFloat(LipBottomInOut);
            packet.WriteFloat(LipBottomInOutL);
            packet.WriteFloat(LipBottomInOutR);
            packet.WriteFloat(LipBottomUpDown);
            packet.WriteFloat(LipBottomUpDownL);
            packet.WriteFloat(LipBottomUpDownR);
            packet.WriteFloat(JawCurve);
            packet.WriteFloat(JawHeight);
            packet.WriteFloat(JawlineDepth);
            packet.WriteFloat(lHornFrontHigh);
            packet.WriteFloat(lHornFrontLarge);
            packet.WriteFloat(lHornFrontSmall);
            packet.WriteFloat(lHornSide);
            packet.WriteFloat(rHornFrontHigh);
            packet.WriteFloat(rHornFrontLarge);
            packet.WriteFloat(rHornFrontSmall);
            packet.WriteFloat(rHornSide);
            packet.WriteFloat(EyelashesLength);
            packet.WriteFloat(EyelidSize);
            packet.WriteFloat(EyelidsFoldDown);
            packet.WriteFloat(EyesAlmondInner);
            packet.WriteFloat(EyesAlmondOuter);
            packet.WriteFloat(EyesAngledInner);
            packet.WriteFloat(EyesAngledOuter);
            packet.WriteFloat(EyesHeight);
            packet.WriteFloat(EyesIrisSize);
            packet.WriteFloat(EyesSize);
            packet.WriteFloat(EyesWidth);
            packet.WriteFloat(EyelidsLowerUpDown);
            packet.WriteFloat(EyelidsLowerUpDownL);
            packet.WriteFloat(EyelidsLowerUpDownR);
            packet.WriteFloat(EyelidsUpperDownUp);
            packet.WriteFloat(EyelidsUpperDownUpL);
            packet.WriteFloat(EyelidsUpperDownUpR);
            packet.WriteFloat(EyesSquint);
            packet.WriteFloat(EyesSquintL);
            packet.WriteFloat(EyesSquintR);
            packet.WriteFloat(EyeSagTop);
            packet.WriteFloat(FaceCenterDepth);
            packet.WriteFloat(FaceFlat);
            packet.WriteFloat(FaceHeart);
            packet.WriteFloat(FaceRound);
            packet.WriteFloat(FaceSquare);
            packet.WriteFloat(BrowInnerUpDown);
            packet.WriteFloat(BrowInnerUpDownL);
            packet.WriteFloat(BrowInnerUpDownR);
            packet.WriteFloat(BrowOuterUpDown);
            packet.WriteFloat(BrowOuterUpDownL);
            packet.WriteFloat(BrowOuterUpDownR);
            packet.WriteFloat(BrowUpDown);
            packet.WriteFloat(BrowUpDownL);
            packet.WriteFloat(BrowUpDownR);
            packet.WriteFloat(CheekbonesDepressionHD);
            packet.WriteFloat(CheeksDepth);
            packet.WriteFloat(ChinCleftHD);
            packet.WriteFloat(ChinWidth);
            packet.WriteFloat(LipsPart);
            packet.WriteFloat(LipsPartCenter);
            packet.WriteFloat(LipsPucker);
            packet.WriteFloat(LipTopUpDown);
            packet.WriteFloat(LipTopUpDownL);
            packet.WriteFloat(LipTopUpDownR);
            packet.WriteFloat(MouthCornerUpDown);
            packet.WriteFloat(MouthFrown);
            packet.WriteFloat(MouthNarrow);
            packet.WriteFloat(MouthNarrowL);
            packet.WriteFloat(MouthNarrowR);
            packet.WriteFloat(MouthCurves);
            packet.WriteFloat(MouthHeight);
            packet.WriteFloat(MouthSize);
            packet.WriteFloat(MouthWidth);
            packet.WriteFloat(LipDepthLower);
            packet.WriteFloat(LipsHeart);
            packet.WriteFloat(LipsSquare);
            packet.WriteFloat(LipUpperSize);
            packet.WriteFloat(NoseAge);
            packet.WriteFloat(NoseBridgeDepth);
            packet.WriteFloat(NoseBridgeHeight);
            packet.WriteFloat(NoseBridgeSkew);
            packet.WriteFloat(NoseBridgeSlope);
            packet.WriteFloat(NoseBump);
            packet.WriteFloat(NoseDepth);
            packet.WriteFloat(NoseHeight);
            packet.WriteFloat(NosePinch);
            packet.WriteFloat(NoseRidgeWidth);
            packet.WriteFloat(NoseSize);
            packet.WriteFloat(NoseTipHeight);
            packet.WriteFloat(NoseTipRound);
            packet.WriteFloat(NoseWidth);
            packet.WriteFloat(NostrilWingSize);
            packet.WriteFloat(NostrilWingWidth);
            packet.WriteFloat(UpperArmsSize);
            packet.WriteFloat(ArmsLength);
            packet.WriteFloat(FingersLength);
            packet.WriteFloat(FingersLengthL);
            packet.WriteFloat(FingersLengthR);
            packet.WriteFloat(FingersWidth);
            packet.WriteFloat(FingersWidthL);
            packet.WriteFloat(FingersWidthR);
            packet.WriteFloat(ForearmsSize);
            packet.WriteFloat(PalmScale);
            packet.WriteFloat(PalmScaleL);
            packet.WriteFloat(PalmScaleR);
            packet.WriteFloat(BasicWeight1);
            packet.WriteFloat(BodybuilderSize);
            packet.WriteFloat(BodybuilderDetails);
            packet.WriteFloat(FitnessSize);
            packet.WriteFloat(FitnessDetails);
            packet.WriteFloat(NeckWeight);
            packet.WriteFloat(NeckLength);
            packet.WriteFloat(NeckSize);
            packet.WriteFloat(ChestDepth);
            packet.WriteFloat(ChestWidth);
            packet.WriteFloat(ShldrsScale);
            packet.WriteFloat(ShldrsWidth);
            packet.WriteFloat(ShoulderDrop);
            packet.WriteFloat(ShouldersSize);
            packet.WriteFloat(PectoralsCleavage);
            packet.WriteFloat(PectoralsDiameter);
            packet.WriteFloat(PectoralsHeavy);
            packet.WriteFloat(PectoralsSag);
            packet.WriteFloat(RibcageSize);
            packet.WriteFloat(SternumDepth);
            packet.WriteFloat(Belly);
            packet.WriteFloat(StomachLowerDepth);
            packet.WriteFloat(ThighsSize);
            packet.WriteFloat(CalvesSize);
            packet.WriteFloat(GlutesSize);
            packet.WriteFloat(LegsLength);
            packet.WriteFloat(ShinsSize);
        }
    }
}