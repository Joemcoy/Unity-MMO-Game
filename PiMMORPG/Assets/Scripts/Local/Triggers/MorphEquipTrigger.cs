using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using MORPH3D;
using MORPH3D.COSTUMING;
using MORPH3D.FOUNDATIONS;
using MarkLight;
using PiMMORPG.Models;
using Scripts.Local.Inventory;
using Scripts.Local.Morph;
using MarkLight.Views.UI;
using tFramework.Extensions;
using Scripts.Local.Bundles;

namespace Scripts.Local.Triggers
{
    [RequireComponent(typeof(M3DCharacterManager))]
    [RequireComponent(typeof(HairSetter))]
    public class MorphEquipTrigger : InventoryEquipTrigger
    {
        M3DCharacterManager Manager;
        public CreateCharacterView View;

        HairSetter _Setter;
        public HairSetter Setter { get { return _Setter == null ? _Setter = GetComponent<HairSetter>() : _Setter; } }

        #region Morphs
        public MorphStyle PHMCraniumSlope = new MorphStyle("Head", "PHMCraniumSlope", (m, v) => m.CraniumSlope = v, m => m.CraniumSlope, -100, 100) { Alternative = "PHMCraniumSlope_NEGATIVE_" };
        public MorphStyle PHMEarlobesAttached = new MorphStyle("Head", "PHMEarlobesAttached", (m, v) => m.EarlobesAttached = v, m => m.EarlobesAttached);
        public MorphStyle PHMEarsDepth = new MorphStyle("Head", "PHMEarsDepth", (m, v) => m.EarsDepth = v, m => m.EarsDepth, -100, 100) { Alternative = "PHMEarsDepth_NEGATIVE_" };
        public MorphStyle PHMEarsElf = new MorphStyle("Head", "PHMEarsElf", (m, v) => m.EarsElf = v, m => m.EarsElf);
        public MorphStyle PHMEarsHeight = new MorphStyle("Head", "PHMEarsHeight", (m, v) => m.EarsHeight = v, m => m.EarsHeight, -100, 100) { Alternative = "PHMEarsHeight_NEGATIVE_" };
        public MorphStyle eCTRLJawOut_In = new MorphStyle("Head", "eCTRLJawOut_In", (m, v) => m.JawOutIn = v, m => m.JawOutIn, -100, 100) { Alternative = "eCTRLJawOut_In_NEGATIVE_" };
        public MorphStyle eCTRLLipBottomIn_Out = new MorphStyle("Head", "eCTRLLipBottomIn_Out", (m, v) => m.LipBottomInOut = v, m => m.LipBottomInOut, -50, 50);
        public MorphStyle eCTRLLipBottomIn_OutL = new MorphStyle("Head", "eCTRLLipBottomIn_OutL", (m, v) => m.LipBottomInOutL = v, m => m.LipBottomInOutL, -50, 50) { Alternative = "eCTRLLipBottomIn_OutL_NEGATIVE_" };
        public MorphStyle eCTRLLipBottomIn_OutR = new MorphStyle("Head", "eCTRLLipBottomIn_OutR", (m, v) => m.LipBottomInOutR = v, m => m.LipBottomInOutR, -50, 50) { Alternative = "eCTRLLipBottomIn_OutR_NEGATIVE_" };
        public MorphStyle eCTRLLipBottomUp_Down = new MorphStyle("Head", "eCTRLLipBottomUp_Down", (m, v) => m.LipBottomUpDown = v, m => m.LipBottomUpDown, 0, 50);
        public MorphStyle eCTRLLipBottomUp_DownL = new MorphStyle("Head", "eCTRLLipBottomUp_DownL", (m, v) => m.LipBottomUpDownL = v, m => m.LipBottomUpDownL, -40, 50) { Alternative = "eCTRLLipBottomUp_DownL_NEGATIVE_" };
        public MorphStyle eCTRLLipBottomUp_DownR = new MorphStyle("Head", "eCTRLLipBottomUp_DownR", (m, v) => m.LipBottomUpDownR = v, m => m.LipBottomUpDownR, -50, 50) { Alternative = "eCTRLLipBottomUp_DownR_NEGATIVE_" };
        public MorphStyle PHMJawCurve = new MorphStyle("Head", "PHMJawCurve", (m, v) => m.JawCurve = v, m => m.JawCurve, -100, 100) { Alternative = "PHMJawCurve_NEGATIVE_" };
        public MorphStyle PHMJawHeight = new MorphStyle("Head", "PHMJawHeight", (m, v) => m.JawHeight = v, m => m.JawHeight, -100, 100) { Alternative = "PHMJawHeight_NEGATIVE_" };
        public MorphStyle PHMJawlineDepth = new MorphStyle("Head", "PHMJawlineDepth", (m, v) => m.JawlineDepth = v, m => m.JawlineDepth, -100, 100) { Alternative = "PHMJawlineDepth_NEGATIVE_" };
        public MorphStyle G2M_PHMlHornFrontHigh = new MorphStyle("Head", "G2M_PHMlHornFrontHigh", (m, v) => m.lHornFrontHigh = v, m => m.lHornFrontHigh);
        public MorphStyle G2M_PHMlHornFrontLarge = new MorphStyle("Head", "G2M_PHMlHornFrontLarge", (m, v) => m.lHornFrontLarge = v, m => m.lHornFrontLarge);
        public MorphStyle G2M_PHMlHornFrontSmall = new MorphStyle("Head", "G2M_PHMlHornFrontSmall", (m, v) => m.lHornFrontSmall = v, m => m.lHornFrontSmall);
        public MorphStyle G2M_PHMlHornSide = new MorphStyle("Head", "G2M_PHMlHornSide", (m, v) => m.lHornSide = v, m => m.lHornSide);
        public MorphStyle G2M_PHMrHornFrontHigh = new MorphStyle("Head", "G2M_PHMrHornFrontHigh", (m, v) => m.rHornFrontHigh = v, m => m.rHornFrontHigh);
        public MorphStyle G2M_PHMrHornFrontLarge = new MorphStyle("Head", "G2M_PHMrHornFrontLarge", (m, v) => m.rHornFrontLarge = v, m => m.rHornFrontLarge);
        public MorphStyle G2M_PHMrHornFrontSmall = new MorphStyle("Head", "G2M_PHMrHornFrontSmall", (m, v) => m.rHornFrontSmall = v, m => m.rHornFrontSmall);
        public MorphStyle G2M_PHMrHornSide = new MorphStyle("Head", "G2M_PHMrHornSide", (m, v) => m.rHornSide = v, m => m.rHornSide);
        public MorphStyle PHMEyelashesLength = new MorphStyle("Eyes", "PHMEyelashesLength", (m, v) => m.EyelashesLength = v, m => m.EyelashesLength);
        public MorphStyle Eyelid_Size = new MorphStyle("Eyes", "Eyelid_Size", (m, v) => m.EyelidSize = v, m => m.EyelidSize);
        public MorphStyle PHMEyelidsFoldDown = new MorphStyle("Eyes", "PHMEyelidsFoldDown", (m, v) => m.EyelidsFoldDown = v, m => m.EyelidsFoldDown, 0, 50);
        public MorphStyle PHMEyesAlmondInner = new MorphStyle("Eyes", "PHMEyesAlmondInner", (m, v) => m.EyesAlmondInner = v, m => m.EyesAlmondInner);
        public MorphStyle PHMEyesAlmondOuter = new MorphStyle("Eyes", "PHMEyesAlmondOuter", (m, v) => m.EyesAlmondOuter = v, m => m.EyesAlmondOuter);
        public MorphStyle PHMEyesAngledInner = new MorphStyle("Eyes", "PHMEyesAngledInner", (m, v) => m.EyesAngledInner = v, m => m.EyesAngledInner, -50, 50) { Alternative = "PHMEyesAngledInner_NEGATIVE_" };
        public MorphStyle PHMEyesAngledOuter = new MorphStyle("Eyes", "PHMEyesAngledOuter", (m, v) => m.EyesAngledOuter = v, m => m.EyesAngledOuter, -50, 50) { Alternative = "PHMEyesAngledOuter_NEGATIVE_" };
        public MorphStyle PHMEyesHeight = new MorphStyle("Eyes", "PHMEyesHeight", (m, v) => m.EyesHeight = v, m => m.EyesHeight, -100, 100) { Alternative = "PHMEyesHeight_NEGATIVE_" };
        public MorphStyle PHMEyesIrisSize = new MorphStyle("Eyes", "PHMEyesIrisSize", (m, v) => m.EyesIrisSize = v, m => m.EyesIrisSize, -50, 50) { Alternative = "PHMEyesIrisSize_NEGATIVE_" };
        public MorphStyle PHMEyesSize = new MorphStyle("Eyes", "PHMEyesSize", (m, v) => m.EyesSize = v, m => m.EyesSize, -50, 25) { Alternative = "PHMEyesSize_NEGATIVE_" };
        public MorphStyle PHMEyesWidth = new MorphStyle("Eyes", "PHMEyesWidth", (m, v) => m.EyesWidth = v, m => m.EyesWidth, -50, 50) { Alternative = "PHMEyesWidth_NEGATIVE_" };
        public MorphStyle eCTRLEyelidsLowerUpDown = new MorphStyle("Eyes", "eCTRLEyelidsLowerUpDown", (m, v) => m.EyelidsLowerUpDown = v, m => m.EyelidsLowerUpDown);
        public MorphStyle eCTRLEyelidsLowerUpDownL = new MorphStyle("Eyes", "eCTRLEyelidsLowerUpDownL", (m, v) => m.EyelidsLowerUpDownL = v, m => m.EyelidsLowerUpDownL, -100, 100) { Alternative = "eCTRLEyelidsLowerUpDownL_NEGATIVE_" };
        public MorphStyle eCTRLEyelidsLowerUpDownR = new MorphStyle("Eyes", "eCTRLEyelidsLowerUpDownR", (m, v) => m.EyelidsLowerUpDownR = v, m => m.EyelidsLowerUpDownR, -100, 100) { Alternative = "eCTRLEyelidsLowerUpDownR_NEGATIVE_" };
        public MorphStyle eCTRLEyelidsUpperDownUp = new MorphStyle("Eyes", "eCTRLEyelidsUpperDownUp", (m, v) => m.EyelidsUpperDownUp = v, m => m.EyelidsUpperDownUp, 0, 25);
        public MorphStyle eCTRLEyelidsUpperDownUpL = new MorphStyle("Eyes", "eCTRLEyelidsUpperDownUpL", (m, v) => m.EyelidsUpperDownUpL = v, m => m.EyelidsUpperDownUpL, 0, 25) { Alternative = "eCTRLEyelidsUpperDownUpL_NEGATIVE_" };
        public MorphStyle eCTRLEyelidsUpperDownUpR = new MorphStyle("Eyes", "eCTRLEyelidsUpperDownUpR", (m, v) => m.EyelidsUpperDownUpR = v, m => m.EyelidsUpperDownUpR, 0, 25) { Alternative = "eCTRLEyelidsUpperDownUpR_NEGATIVE_" };
        public MorphStyle eCTRLEyesSquint = new MorphStyle("Eyes", "eCTRLEyesSquint", (m, v) => m.EyesSquint = v, m => m.EyesSquint);
        public MorphStyle eCTRLEyesSquintL = new MorphStyle("Eyes", "eCTRLEyesSquintL", (m, v) => m.EyesSquintL = v, m => m.EyesSquintL);
        public MorphStyle eCTRLEyesSquintR = new MorphStyle("Eyes", "eCTRLEyesSquintR", (m, v) => m.EyesSquintR = v, m => m.EyesSquintR);
        public MorphStyle Eye_Sag_Top = new MorphStyle("Eyes", "Eye_Sag_Top", (m, v) => m.EyeSagTop = v, m => m.EyeSagTop);
        public MorphStyle PHMFaceCenterDepth = new MorphStyle("Face", "PHMFaceCenterDepth", (m, v) => m.FaceCenterDepth = v, m => m.FaceCenterDepth, 0, 50);
        public MorphStyle PHMFaceFlat = new MorphStyle("Face", "PHMFaceFlat", (m, v) => m.FaceFlat = v, m => m.FaceFlat, -50, 100) { Alternative = "PHMFaceFlat_NEGATIVE_" };
        public MorphStyle PHMFaceHeart = new MorphStyle("Face", "PHMFaceHeart", (m, v) => m.FaceHeart = v, m => m.FaceHeart, -50, 100) { Alternative = "PHMFaceHeart_NEGATIVE_" };
        public MorphStyle PHMFaceRound = new MorphStyle("Face", "PHMFaceRound", (m, v) => m.FaceRound = v, m => m.FaceRound, -50, 100) { Alternative = "PHMFaceRound_NEGATIVE_" };
        public MorphStyle PHMFaceSquare = new MorphStyle("Face", "PHMFaceSquare", (m, v) => m.FaceSquare = v, m => m.FaceSquare, -50, 100) { Alternative = "PHMFaceSquare_NEGATIVE" };
        public MorphStyle eCTRLBrowInnerUp_Down = new MorphStyle("Face", "eCTRLBrowInnerUp_Down", (m, v) => m.BrowInnerUpDown = v, m => m.BrowInnerUpDown);
        public MorphStyle eCTRLBrowInnerUp_DownL = new MorphStyle("Face", "eCTRLBrowInnerUp_DownL", (m, v) => m.BrowInnerUpDownL = v, m => m.BrowInnerUpDownL, -100, 100) { Alternative = "eCTRLBrowInnerUp_DownL_NEGATIVE_" };
        public MorphStyle eCTRLBrowInnerUp_DownR = new MorphStyle("Face", "eCTRLBrowInnerUp_DownR", (m, v) => m.BrowInnerUpDownR = v, m => m.BrowInnerUpDownR, -100, 100) { Alternative = "eCTRLBrowInnerUp_DownR_NEGATIVE_" };
        public MorphStyle eCTRLBrowOuterUp_Down = new MorphStyle("Face", "eCTRLBrowOuterUp_Down", (m, v) => m.BrowOuterUpDown = v, m => m.BrowOuterUpDown);
        public MorphStyle eCTRLBrowOuterUp_DownL = new MorphStyle("Face", "eCTRLBrowOuterUp_DownL", (m, v) => m.BrowOuterUpDownL = v, m => m.BrowOuterUpDownL);
        public MorphStyle eCTRLBrowOuterUp_DownR = new MorphStyle("Face", "eCTRLBrowOuterUp_DownR", (m, v) => m.BrowOuterUpDownR = v, m => m.BrowOuterUpDownR);
        public MorphStyle eCTRLBrowUp_Down = new MorphStyle("Face", "eCTRLBrowUp_Down", (m, v) => m.BrowUpDown = v, m => m.BrowUpDown);
        public MorphStyle eCTRLBrowUp_DownL = new MorphStyle("Face", "eCTRLBrowUp_DownL", (m, v) => m.BrowUpDownL = v, m => m.BrowUpDownL, -100, 100) { Alternative = "eCTRLBrowUp_DownL_NEGATIVE_" };
        public MorphStyle eCTRLBrowUp_DownR = new MorphStyle("Face", "eCTRLBrowUp_DownR", (m, v) => m.BrowUpDownR = v, m => m.BrowUpDownR, -100, 100) { Alternative = "eCTRLBrowUp_DownR_NEGATIVE_" };
        public MorphStyle PHMCheekbonesDepressionHD = new MorphStyle("Face", "PHMCheekbonesDepressionHD", (m, v) => m.CheekbonesDepressionHD = v, m => m.CheekbonesDepressionHD);
        public MorphStyle PHMCheeksDepth = new MorphStyle("Face", "PHMCheeksDepth", (m, v) => m.CheeksDepth = v, m => m.CheeksDepth);
        public MorphStyle PHMChinCleftHD = new MorphStyle("Face", "PHMChinCleftHD", (m, v) => m.ChinCleftHD = v, m => m.ChinCleftHD);
        public MorphStyle PHMChinWidth = new MorphStyle("Face", "PHMChinWidth", (m, v) => m.ChinWidth = v, m => m.ChinWidth);
        public MorphStyle eCTRLLipsPart = new MorphStyle("Face", "eCTRLLipsPart", (m, v) => m.LipsPart = v, m => m.LipsPart);
        public MorphStyle eCTRLLipsPartCenter = new MorphStyle("Face", "eCTRLLipsPartCenter", (m, v) => m.LipsPartCenter = v, m => m.LipsPartCenter);
        public MorphStyle eCTRLLipsPucker = new MorphStyle("Face", "eCTRLLipsPucker", (m, v) => m.LipsPucker = v, m => m.LipsPucker);
        public MorphStyle eCTRLLipTopUp_Down = new MorphStyle("Face", "eCTRLLipTopUp_Down", (m, v) => m.LipTopUpDown = v, m => m.LipTopUpDown);
        public MorphStyle eCTRLLipTopUp_DownL = new MorphStyle("Face", "eCTRLLipTopUp_DownL", (m, v) => m.LipTopUpDownL = v, m => m.LipTopUpDownL, -100, 100) { Alternative = "eCTRLLipTopUp_DownL_NEGATIVE_" };
        public MorphStyle eCTRLLipTopUp_DownR = new MorphStyle("Face", "eCTRLLipTopUp_DownR", (m, v) => m.LipTopUpDownR = v, m => m.LipTopUpDownR, -100, 100) { Alternative = "eCTRLLipTopUp_DownR_NEGATIVE_" };
        public MorphStyle eCTRLMouthCornerUp_Down = new MorphStyle("Face", "eCTRLMouthCornerUp_Down", (m, v) => m.MouthCornerUpDown = v, m => m.MouthCornerUpDown, 0, 50);
        public MorphStyle eCTRLMouthFrown = new MorphStyle("Face", "eCTRLMouthFrown", (m, v) => m.MouthFrown = v, m => m.MouthFrown, 0, 50);
        public MorphStyle eCTRLMouthNarrow = new MorphStyle("Face", "eCTRLMouthNarrow", (m, v) => m.MouthNarrow = v, m => m.MouthNarrow, 0, 75);
        public MorphStyle eCTRLMouthNarrowL = new MorphStyle("Face", "eCTRLMouthNarrowL", (m, v) => m.MouthNarrowL = v, m => m.MouthNarrowL, -50, 50) { Alternative = "eCTRLMouthNarrowL_NEGATIVE_" };
        public MorphStyle eCTRLMouthNarrowR = new MorphStyle("Face", "eCTRLMouthNarrowR", (m, v) => m.MouthNarrowR = v, m => m.MouthNarrowR, -50, 50) { Alternative = "eCTRLMouthNarrowR_NEGATIVE_" };
        public MorphStyle PHMMouthCurves = new MorphStyle("Face", "PHMMouthCurves", (m, v) => m.MouthCurves = v, m => m.MouthCurves, -100, 100) { Alternative = "PHMMouthCurves_NEGATIVE_" };
        public MorphStyle PHMMouthHeight = new MorphStyle("Face", "PHMMouthHeight", (m, v) => m.MouthHeight = v, m => m.MouthHeight, -100, 100) { Alternative = "PHMMouthHeight_NEGATIVE_" };
        public MorphStyle PHMMouthSize = new MorphStyle("Face", "PHMMouthSize", (m, v) => m.MouthSize = v, m => m.MouthSize, -50, 50) { Alternative = "PHMMouthSize_NEGATIVE_" };
        public MorphStyle PHMMouthWidth = new MorphStyle("Face", "PHMMouthWidth", (m, v) => m.MouthWidth = v, m => m.MouthWidth, -100, 100) { Alternative = "PHMMouthWidth_NEGATIVE_" };
        public MorphStyle PHMLipDepthLower = new MorphStyle("Face", "PHMLipDepthLower", (m, v) => m.LipDepthLower = v, m => m.LipDepthLower);
        public MorphStyle PHMLipsHeart = new MorphStyle("Face", "PHMLipsHeart", (m, v) => m.LipsHeart = v, m => m.LipsHeart, -100, 100) { Alternative = "PHMLipsHeart_NEGATIVE_" };
        public MorphStyle PHMLipsSquare = new MorphStyle("Face", "PHMLipsSquare", (m, v) => m.LipsSquare = v, m => m.LipsSquare, -100, 100) { Alternative = "PHMLipsSquare_NEGATIVE_" };
        public MorphStyle PHMLipUpperSize = new MorphStyle("Face", "PHMLipUpperSize", (m, v) => m.LipUpperSize = v, m => m.LipUpperSize, -100, 100) { Alternative = "PHMLipUpperSize_NEGATIVE_" };
        public MorphStyle Nose_Age = new MorphStyle("Body", "Nose_Age", (m, v) => m.NoseAge = v, m => m.NoseAge);
        public MorphStyle PHMNoseBridgeDepth = new MorphStyle("Nose", "PHMNoseBridgeDepth", (m, v) => m.NoseBridgeDepth = v, m => m.NoseBridgeDepth, -100, 100) { Alternative = "PHMNoseBridgeDepth_NEGATIVE_" };
        public MorphStyle PHMNoseBridgeHeight = new MorphStyle("Nose", "PHMNoseBridgeHeight", (m, v) => m.NoseBridgeHeight = v, m => m.NoseBridgeHeight, -100, 100) { Alternative = "PHMNoseBridgeHeight_NEGATIVE_" };
        public MorphStyle PHMNoseBridgeSkew = new MorphStyle("Nose", "PHMNoseBridgeSkew", (m, v) => m.NoseBridgeSkew = v, m => m.NoseBridgeSkew, -100, 100) { Alternative = "PHMNoseBridgeSkew_NEGATIVE_" };
        public MorphStyle PHMNoseBridgeSlope = new MorphStyle("Nose", "PHMNoseBridgeSlope", (m, v) => m.NoseBridgeSlope = v, m => m.NoseBridgeSlope, -100, 100) { Alternative = "PHMNoseBridgeSlope_NEGATIVE_" };
        public MorphStyle PHMNoseBump = new MorphStyle("Nose", "PHMNoseBump", (m, v) => m.NoseBump = v, m => m.NoseBump);
        public MorphStyle PHMNoseDepth = new MorphStyle("Nose", "PHMNoseDepth", (m, v) => m.NoseDepth = v, m => m.NoseDepth) { Alternative = "PHMNoseDepth_NEGATIVE_" };
        public MorphStyle PHMNoseHeight = new MorphStyle("Nose", "PHMNoseHeight", (m, v) => m.NoseHeight = v, m => m.NoseHeight) { Alternative = "PHMNoseHeight_NEGATIVE_" };
        public MorphStyle PHMNosePinch = new MorphStyle("Nose", "PHMNosePinch", (m, v) => m.NosePinch = v, m => m.NosePinch) { Alternative = "PHMNosePinch_NEGATIVE_" };
        public MorphStyle PHMNoseRidgeWidth = new MorphStyle("Nose", "PHMNoseRidgeWidth", (m, v) => m.NoseRidgeWidth = v, m => m.NoseRidgeWidth) { Alternative = "PHMNoseRidgeWidth_NEGATIVE_" };
        public MorphStyle PHMNoseSize = new MorphStyle("Nose", "PHMNoseSize", (m, v) => m.NoseSize = v, m => m.NoseSize) { Alternative = "PHMNoseSize_NEGATIVE_" };
        public MorphStyle PHMNoseTipHeight = new MorphStyle("Nose", "PHMNoseTipHeight", (m, v) => m.NoseTipHeight = v, m => m.NoseTipHeight) { Alternative = "PHMNoseTipHeight_NEGATIVE_" };
        public MorphStyle PHMNoseTipRound = new MorphStyle("Nose", "PHMNoseTipRound", (m, v) => m.NoseTipRound = v, m => m.NoseTipRound) { Alternative = "PHMNoseTipRound_NEGATIVE_" };
        public MorphStyle PHMNoseWidth = new MorphStyle("Nose", "PHMNoseWidth", (m, v) => m.NoseWidth = v, m => m.NoseWidth) { Alternative = "PHMNoseWidth_NEGATIVE_" };
        public MorphStyle PHMNostrilWingSize = new MorphStyle("Nose", "PHMNostrilWingSize", (m, v) => m.NostrilWingSize = v, m => m.NostrilWingSize) { Alternative = "PHMNostrilWingSize_NEGATIVE_" };
        public MorphStyle PHMNostrilWingWidth = new MorphStyle("Nose", "PHMNostrilWingWidth", (m, v) => m.NostrilWingWidth = v, m => m.NostrilWingWidth) { Alternative = "PHMNostrilWingWidth_NEGATIVE_" };
        public MorphStyle PBMUpperArmsSize = new MorphStyle("Arms", "PBMUpperArmsSize", (m, v) => m.UpperArmsSize = v, m => m.UpperArmsSize);
        public MorphStyle SCLArmsLength = new MorphStyle("Arms", "SCLArmsLength", (m, v) => m.ArmsLength = v, m => m.ArmsLength, -100, 100) { Alternative = "SCLArmsLength_NEGATIVE_" };
        public MorphStyle SCLFingersLength = new MorphStyle("Arms", "SCLFingersLength", (m, v) => m.FingersLength = v, m => m.FingersLength, -100, 100) { Alternative = "SCLFingersLength_NEGATIVE_" };
        public MorphStyle SCLFingersLengthL = new MorphStyle("Arms", "SCLFingersLengthL", (m, v) => m.FingersLengthL = v, m => m.FingersLengthL, -100, 100) { Alternative = "SCLFingersLengthL_NEGATIVE_" };
        public MorphStyle SCLFingersLengthR = new MorphStyle("Arms", "SCLFingersLengthR", (m, v) => m.FingersLengthR = v, m => m.FingersLengthR, -100, 100) { Alternative = "SCLFingersLengthR_NEGATIVE_" };
        public MorphStyle SCLFingersWidth = new MorphStyle("Arms", "SCLFingersWidth", (m, v) => m.FingersWidth = v, m => m.FingersWidth, -100, 100) { Alternative = "SCLFingersWidth_NEGATIVE_" };
        public MorphStyle SCLFingersWidthL = new MorphStyle("Arms", "SCLFingersWidthL", (m, v) => m.FingersWidthL = v, m => m.FingersWidthL, -100, 100) { Alternative = "SCLFingersWidthL_NEGATIVE_" };
        public MorphStyle SCLFingersWidthR = new MorphStyle("Arms", "SCLFingersWidthR", (m, v) => m.FingersWidthR = v, m => m.FingersWidthR, -100, 100) { Alternative = "SCLFingersWidthR_NEGATIVE_" };
        public MorphStyle PBMForearmsSize = new MorphStyle("Arms", "PBMForearmsSize", (m, v) => m.ForearmsSize = v, m => m.ForearmsSize, -100, 100) { Alternative = "PBMForearmsSize_NEGATIVE_" };
        public MorphStyle SCLPalmScale = new MorphStyle("Hand", "SCLPalmScale", (m, v) => m.PalmScale = v, m => m.PalmScale) { Alternative = "SCLPalmScale_NEGATIVE_" };
        public MorphStyle SCLPalmScaleL = new MorphStyle("Hand", "SCLPalmScaleL", (m, v) => m.PalmScaleL = v, m => m.PalmScaleL) { Alternative = "SCLPalmScaleL_NEGATIVE_" };
        public MorphStyle SCLPalmScaleR = new MorphStyle("Hand", "SCLPalmScaleR", (m, v) => m.PalmScaleR = v, m => m.PalmScaleR) { Alternative = "SCLPalmScaleR_NEGATIVE_" };
        public MorphStyle Basic_Weight_1 = new MorphStyle("Body", "Basic_Weight_1", (m, v) => m.BasicWeight1 = v, m => m.BasicWeight1);
        public MorphStyle FBMBodybuilderSize = new MorphStyle("Body", "FBMBodybuilderSize", (m, v) => m.BodybuilderSize = v, m => m.BodybuilderSize);
        public MorphStyle FBMBodybuilderDetails = new MorphStyle("Body", "FBMBodybuilderDetails", (m, v) => m.BodybuilderDetails = v, m => m.BodybuilderDetails);
        public MorphStyle FBMFitnessSize = new MorphStyle("Body", "FBMFitnessSize", (m, v) => m.FitnessSize = v, m => m.FitnessSize);
        public MorphStyle FBMFitnessDetails = new MorphStyle("Body", "FBMFitnessDetails", (m, v) => m.FitnessDetails = v, m => m.FitnessDetails);
        public MorphStyle Neck_Weight = new MorphStyle("Body", "Neck_Weight", (m, v) => m.NeckWeight = v, m => m.NeckWeight);
        public MorphStyle SCLNeckLength = new MorphStyle("Body", "SCLNeckLength", (m, v) => m.NeckLength = v, m => m.NeckLength, -100, 100) { Alternative = "SCLNeckLength_NEGATIVE_" };
        public MorphStyle PBMNeckSize = new MorphStyle("Body", "PBMNeckSize", (m, v) => m.NeckSize = v, m => m.NeckSize);
        public MorphStyle SCLChestDepth = new MorphStyle("Torso", "SCLChestDepth", (m, v) => m.ChestDepth = v, m => m.ChestDepth, -100, 100) { Alternative = "SCLChestDepth_NEGATIVE_" };
        public MorphStyle SCLChestWidth = new MorphStyle("Torso", "SCLChestWidth", (m, v) => m.ChestWidth = v, m => m.ChestWidth, -100, 100) { Alternative = "SCLChestWidth_NEGATIVE_" };
        public MorphStyle SCLShldrsScale = new MorphStyle("Torso", "SCLShldrsScale", (m, v) => m.ShldrsScale = v, m => m.ShldrsScale, -50, 100) { Alternative = "SCLShldrsScale_NEGATIVE_" };
        public MorphStyle SCLShldrsWidth = new MorphStyle("Torso", "SCLShldrsWidth", (m, v) => m.ShldrsWidth = v, m => m.ShldrsWidth, -50, 100) { Alternative = "SCLShldrsWidth_NEGATIVE_" };
        public MorphStyle Shoulder_Drop = new MorphStyle("Torso", "Shoulder_Drop", (m, v) => m.ShoulderDrop = v, m => m.ShoulderDrop);
        public MorphStyle PBMShouldersSize = new MorphStyle("Torso", "PBMShouldersSize", (m, v) => m.ShouldersSize = v, m => m.ShouldersSize, 0, 50);
        public MorphStyle PBMPectoralsCleavage = new MorphStyle("Torso", "PBMPectoralsCleavage", (m, v) => m.PectoralsCleavage = v, m => m.PectoralsCleavage);
        public MorphStyle PBMPectoralsDiameter = new MorphStyle("Torso", "PBMPectoralsDiameter", (m, v) => m.PectoralsDiameter = v, m => m.PectoralsDiameter);
        public MorphStyle PBMPectoralsHeavy = new MorphStyle("Torso", "PBMPectoralsHeavy", (m, v) => m.PectoralsHeavy = v, m => m.PectoralsHeavy);
        public MorphStyle PBMPectoralsSag = new MorphStyle("Torso", "PBMPectoralsSag", (m, v) => m.PectoralsSag = v, m => m.PectoralsSag);
        public MorphStyle PBMRibcageSize = new MorphStyle("Torso", "PBMRibcageSize", (m, v) => m.RibcageSize = v, m => m.RibcageSize);
        public MorphStyle PBMSternumDepth = new MorphStyle("Torso", "PBMSternumDepth", (m, v) => m.SternumDepth = v, m => m.SternumDepth);
        public MorphStyle G2M_PBMBelly = new MorphStyle("Torso", "G2M_PBMBelly", (m, v) => m.Belly = v, m => m.Belly);
        public MorphStyle PBMStomachLowerDepth = new MorphStyle("Torso", "PBMStomachLowerDepth", (m, v) => m.StomachLowerDepth = v, m => m.StomachLowerDepth);
        public MorphStyle PBMThighsSize = new MorphStyle("Legs", "PBMThighsSize", (m, v) => m.ThighsSize = v, m => m.ThighsSize);
        public MorphStyle PBMCalvesSize = new MorphStyle("Legs", "PBMCalvesSize", (m, v) => m.CalvesSize = v, m => m.CalvesSize, -100, 100) { Alternative = "PBMCalvesSize_NEGATIVE_" };
        public MorphStyle PBMGlutesSize = new MorphStyle("Legs", "PBMGlutesSize", (m, v) => m.GlutesSize = v, m => m.GlutesSize, -100, 100) { Alternative = "PBMGlutesSize_NEGATIVE_" };
        public MorphStyle PBMLegsLength = new MorphStyle("Legs", "PBMLegsLength", (m, v) => m.LegsLength = v, m => m.LegsLength, -25, 25) { Alternative = "PBMLegsLength_NEGATIVE_" };
        public MorphStyle PBMShinsSize = new MorphStyle("Legs", "PBMShinsSize", (m, v) => m.ShinsSize = v, m => m.ShinsSize);
        #endregion
        public List<MorphStyle> Styles;

        public override void Init(bool IsLocal)
        {
            base.Init(IsLocal);

            /*var fix = BundleLoader.LoadAsset<Texture>("textures/characters/male_fix");
            foreach(var renderer in GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                var material = renderer.sharedMaterials.First(m => m.name.IndexOf("Body_") > -1);
                material.mainTexture = fix;
            }*/

            Manager = GetComponent<M3DCharacterManager>();
            Styles = new List<MorphStyle>();
            foreach (var Field in GetType().GetFields().Where(F => F.FieldType == typeof(MorphStyle)))
            {
                var Style = (MorphStyle)Field.GetValue(this);
                Style.Manager = Manager;

                Styles.Add(Style);
            }
            
            foreach (var camera in GetComponentsInChildren<Camera>())
                camera.enabled = IsLocal;

            foreach (var Hair in Manager.GetAllHair())
                Hair.isVisible = false;
        }

        public override void UnloadEvents()
        {
            base.UnloadEvents();

            if(Manager)
                Manager.RemoveAllContentPacks();
            Reset();
        }

        public override void Equip(NetworkEquippableItem Equip, GameObject visual = null)
        {
            //base.Equip(Equip);

            //var Cloth = Equip.GetComponent<CIclothing>();
            //if (Cloth)
            //{
            //    if (!Cloths.Contains(Cloth.ID))
            //    {
            //        Cloths.Add(Cloth.ID);
            //        Manager.AddContentPack(new ContentPack(Cloth.gameObject));
            //    }
            //    else
            //        Manager.SetClothingVisibility(Cloth.ID, true);
            //}

            if (Equip.GetComponent<CIclothing>() != null)
            {
                try
                {
                    var Pack = new ContentPack(Equip.gameObject);
                    foreach (var aCloth in Pack.availableClothing)
                    {
                        if (Manager.GetClothingByID(aCloth.ID) != null)
                            Manager.SetClothingVisibility(aCloth.ID, true);
                        else
                            Manager.AddContentPack(Pack);

                        var eCloth = Manager.GetClothingByID(aCloth.ID);
                        if (!eCloth.gameObject.activeInHierarchy)
                            eCloth.gameObject.SetActive(true);

                        var group = eCloth.GetComponentInChildren<LODGroup>();
                        if (group && group.enabled)
                            group.enabled = false;

                        /*foreach (var renderer in eCloth.GetComponentsInChildren<SkinnedMeshRenderer>())
                            if (!renderer.enabled)
                                renderer.enabled = true;*/
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            else
                base.Equip(Equip);
        }

        public override void UnEquip(NetworkEquippableItem Item, bool Remove = true, bool NewEquip = false)
        {
            base.UnEquip(Item, Remove, NewEquip);

            var Cloth = Item.GetComponentInChildren<CIclothing>();
            if (Cloth != null && Manager != null)
            {
                var rCloth = Manager.GetClothingByID(Cloth.ID);
                if(rCloth != null)
                {
                    rCloth.SetVisibility(false);
                }
            }
        }        

        public void UnEquipAll()
        {
            if (Manager)
                foreach (var cloth in Manager.GetAllClothing())
                    cloth.SetVisibility(false);
        }

        public void CopyFrom(CharacterStyle Style)
        {
            if (!Initalized)
                Init(IsLocal);
            
            Setter.SetHair(Style.Hair, () => SetHairColor(Style.HairColor));
            Styles.ForEach(s => s.CopyFrom(Style));
        }

        void SetHairColor(short color)
        {
            if(Setter.CurrentIndex > -1)
                Setter.Current.Current = color;
        }

        public void CopyTo(CharacterStyle Style)
        {
            Style.Hair = Setter.CurrentIndex;
            Style.HairColor = Setter.CurrentIndex > -1 ? Setter.Current.Current : (short)0;
            Styles.ForEach(s => s.CopyTo(Style));
        }

        public void Reset()
        {
            if(Styles != null)
                Styles.Where(s => s.Changed).ForEach(Reset);
            UnEquipAll();
        }

        void Reset(MorphStyle Style)
        {
            Style.Value.Value = 0f;

            if(View)
                View.GetChildren<Slider>(v => v.Id == Style.Morph).ForEach(v => v.Value.Value = 0f);
        }
    }
}