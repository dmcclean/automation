﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Collections;
using AutomationLibrary.Mathematics;
using AutomationLibrary.Mathematics.Fitting;
using AutomationLibrary.Mathematics.Geometry;
using AutomationLibrary.Mathematics.ProfileSpecification;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {


            List<Vector2> points = new List<Vector2>();

            points.Add(new Vector2(38.9238998652099, -14.4083817692965));
            points.Add(new Vector2(39.4558176028221, -23.9475229408115));
            points.Add(new Vector2(36.1758917917507, -14.1189799708327));
            points.Add(new Vector2(38.8471644121941, -14.4626525160227));
            points.Add(new Vector2(32.3908865263507, -17.5606100312036));
            points.Add(new Vector2(34.9186856266133, -14.7670707807105));
            points.Add(new Vector2(32.5022538622275, -18.0090899569037));
            points.Add(new Vector2(33.3134728499589, -22.3465736958773));
            points.Add(new Vector2(35.5661734368389, -24.2334261008941));
            points.Add(new Vector2(42.519236947004, -18.8779409540853));
            points.Add(new Vector2(34.5406946037444, -15.017175540764));
            points.Add(new Vector2(42.5074836953381, -20.6263242494059));
            points.Add(new Vector2(32.2059539984758, -20.492529053821));
            points.Add(new Vector2(32.4297038059244, -17.6633063761359));
            points.Add(new Vector2(42.5877007494396, -18.0026638311908));
            points.Add(new Vector2(40.3804936922242, -15.1797931359037));
            points.Add(new Vector2(38.6604314796997, -24.2041445745425));
            points.Add(new Vector2(40.0753305889908, -23.664283412022));
            points.Add(new Vector2(39.9259032405698, -23.7390199020844));
            points.Add(new Vector2(32.2721869326112, -19.5164286745049));
            points.Add(new Vector2(33.0193744213178, -21.9222033940682));
            points.Add(new Vector2(42.1242234190001, -17.4275371864543));
            points.Add(new Vector2(36.8003473036929, -14.1368285980791));
            points.Add(new Vector2(34.5365039575565, -15.0986875632496));
            points.Add(new Vector2(32.1993847392008, -19.2391539403059));
            points.Add(new Vector2(39.8558916830103, -23.8131886589329));
            points.Add(new Vector2(33.3116734712761, -22.6525577622359));
            points.Add(new Vector2(35.4995590297645, -24.0075759115719));
            points.Add(new Vector2(36.1273465885746, -24.4902818714164));
            points.Add(new Vector2(33.2092331116385, -22.2406450106769));
            points.Add(new Vector2(42.6527896671612, -18.4341411551654));
            points.Add(new Vector2(42.7403527881813, -19.6240087025994));
            points.Add(new Vector2(32.1616689023408, -20.1387183750112));
            points.Add(new Vector2(41.6863100640054, -16.5231353064129));
            points.Add(new Vector2(33.0297467350975, -16.7021525726927));
            points.Add(new Vector2(32.1452722620169, -18.5996445280811));
            points.Add(new Vector2(32.3053862231585, -18.6636907039533));
            points.Add(new Vector2(32.1887423180084, -18.4399465666238));
            points.Add(new Vector2(40.1943296447452, -23.8271767979109));
            points.Add(new Vector2(40.7829888892626, -15.5269383730031));
            points.Add(new Vector2(41.2143992347765, -15.945365347146));
            points.Add(new Vector2(41.0101585157907, -15.573066398802));
            points.Add(new Vector2(33.0425673327351, -22.1383702210065));
            points.Add(new Vector2(36.3786482427023, -14.1185617478364));
            points.Add(new Vector2(34.5193662279844, -14.9310130275024));
            points.Add(new Vector2(42.1919189189454, -17.2131621338273));
            points.Add(new Vector2(41.4554392902157, -22.384376364459));
            points.Add(new Vector2(39.569656565396, -24.1345470560848));
            points.Add(new Vector2(32.3707982206662, -17.9237798065489));
            points.Add(new Vector2(42.3405695529662, -20.3627913850551));
            points.Add(new Vector2(38.6298504430076, -14.099194350966));
            points.Add(new Vector2(42.2983999332485, -17.4994995773649));
            points.Add(new Vector2(37.7427752230965, -24.4295744708133));
            points.Add(new Vector2(40.021694221823, -23.7375142430049));
            points.Add(new Vector2(39.4175011457591, -14.6551530076213));
            points.Add(new Vector2(32.7030549815251, -17.0641531563988));
            points.Add(new Vector2(42.0900920139613, -16.8173252866671));
            points.Add(new Vector2(42.3963129267243, -20.5250993922335));
            points.Add(new Vector2(36.0411114285429, -24.4079123219998));
            points.Add(new Vector2(32.3511407988561, -19.8403019118994));
            points.Add(new Vector2(32.2600036836219, -18.0928446724938));
            points.Add(new Vector2(42.0120352037419, -17.0806747840519));
            points.Add(new Vector2(39.5812430467095, -23.8687628615713));
            points.Add(new Vector2(39.8847213987367, -14.740138320159));
            points.Add(new Vector2(38.2108400084927, -24.3829844674513));
            points.Add(new Vector2(32.2404874623699, -19.4910129014079));
            points.Add(new Vector2(36.7191900236973, -14.1767629183084));
            points.Add(new Vector2(41.8198793471871, -16.3406009025741));
            points.Add(new Vector2(42.1495366216126, -16.9638253248345));
            points.Add(new Vector2(34.6266649029382, -15.0065077732493));
            points.Add(new Vector2(33.1263408035717, -22.2091982557051));
            points.Add(new Vector2(38.2408898916998, -24.5454950227647));
            points.Add(new Vector2(36.7276706288858, -24.3223353822318));
            points.Add(new Vector2(36.4716850141294, -24.455992132532));
            points.Add(new Vector2(39.3066245832414, -14.3853624490527));
            points.Add(new Vector2(36.1265608528758, -24.3590716817812));
            points.Add(new Vector2(32.5809863309865, -17.147259711632));
            points.Add(new Vector2(41.5664293070484, -22.5695796035351));
            points.Add(new Vector2(34.0472652552698, -23.1181096317448));
            points.Add(new Vector2(37.3599515908012, -14.2050936767252));
            points.Add(new Vector2(39.6173010459533, -23.9010998524411));
            points.Add(new Vector2(40.8990597937742, -23.0623647794901));
            points.Add(new Vector2(33.6778723845709, -22.9682528599053));
            points.Add(new Vector2(42.0332188822954, -16.9175084452959));
            points.Add(new Vector2(36.7131804332678, -24.5609215580186));
            points.Add(new Vector2(36.0120143932191, -24.187617424397));
            points.Add(new Vector2(32.295111338246, -20.5734924469337));
            points.Add(new Vector2(36.1465765887018, -14.1410039455953));
            points.Add(new Vector2(42.0759060603304, -17.063175761391));
            points.Add(new Vector2(32.9452965741679, -16.9188985880144));
            points.Add(new Vector2(34.1870845998, -23.1996496281657));
            points.Add(new Vector2(40.6990744170921, -15.2847844771172));
            points.Add(new Vector2(34.4859302017381, -14.918792578109));
            points.Add(new Vector2(32.1523520549037, -20.2437424903712));
            points.Add(new Vector2(41.2754885827093, -22.854759625316));
            points.Add(new Vector2(33.4715529458796, -15.6772176929931));
            points.Add(new Vector2(37.5669602192874, -24.5760540093802));
            points.Add(new Vector2(39.2828600660975, -14.4904309416767));
            points.Add(new Vector2(34.3649724001037, -15.2678285769043));
            points.Add(new Vector2(38.1714281703214, -14.0015392407297));
            points.Add(new Vector2(42.7119803580736, -19.9789947034517));
            points.Add(new Vector2(32.3420581594547, -20.8725017085627));
            points.Add(new Vector2(41.9535648857193, -16.8401905351044));
            points.Add(new Vector2(32.1270147207584, -19.4800283999365));
            points.Add(new Vector2(32.7583193289139, -21.8744843246762));
            points.Add(new Vector2(42.2298616297851, -21.5538593204588));
            points.Add(new Vector2(41.5519782295135, -16.4007910337709));
            points.Add(new Vector2(34.0514940536682, -15.457040458941));
            points.Add(new Vector2(38.9005611345959, -14.3085752009945));
            points.Add(new Vector2(34.1574262751497, -15.2165147287812));
            points.Add(new Vector2(38.8621766444356, -14.2831216356677));
            points.Add(new Vector2(38.6803839828152, -24.3216966966624));
            points.Add(new Vector2(36.7881634836455, -14.2678362565704));
            points.Add(new Vector2(34.9336299863432, -14.7634410237257));
            points.Add(new Vector2(38.2132078457591, -24.4614289418934));
            points.Add(new Vector2(38.9185429660244, -24.1159163792779));
            points.Add(new Vector2(41.661975322435, -22.3532946634175));
            points.Add(new Vector2(37.2769793571646, -24.5142213342485));
            points.Add(new Vector2(42.7042499708321, -19.4831746547923));
            points.Add(new Vector2(42.5065129993694, -19.5351917779069));
            points.Add(new Vector2(37.150741279526, -24.4745858014442));
            points.Add(new Vector2(32.4526092595657, -17.2495986962829));
            points.Add(new Vector2(35.2211511583214, -24.154926322284));
            points.Add(new Vector2(41.655870435865, -22.1166417882156));
            points.Add(new Vector2(41.8369619003738, -16.6544480727975));
            points.Add(new Vector2(35.1296142121982, -14.7851883758312));
            points.Add(new Vector2(36.7991273341498, -24.5082908431528));
            points.Add(new Vector2(42.7213937524631, -19.889126648683));
            points.Add(new Vector2(33.6800657290046, -23.0745179733247));
            points.Add(new Vector2(38.9233461657838, -14.1830018479485));
            points.Add(new Vector2(39.7967681953265, -14.7847818372936));
            points.Add(new Vector2(40.1303147970978, -14.7664664098051));
            points.Add(new Vector2(40.1937914225425, -23.6215493972679));
            points.Add(new Vector2(40.1977388821304, -14.8040831546672));
            points.Add(new Vector2(40.9568975546658, -23.2594211244621));
            points.Add(new Vector2(41.0940999494581, -15.7680446268724));
            points.Add(new Vector2(42.6853925268779, -19.4936856296067));
            points.Add(new Vector2(33.2898452207168, -22.3998949401538));
            points.Add(new Vector2(39.652412605001, -23.8506145137423));
            points.Add(new Vector2(41.6713202166943, -16.1889426433625));
            points.Add(new Vector2(37.8030705034284, -24.4673647436786));
            points.Add(new Vector2(42.0204706903422, -21.9656915123596));
            points.Add(new Vector2(40.7538691917861, -15.2577564464137));
            points.Add(new Vector2(33.3792934060871, -16.0183007961632));
            points.Add(new Vector2(32.3059288978376, -19.0507101192803));
            points.Add(new Vector2(37.6884563294828, -24.5921772196701));
            points.Add(new Vector2(32.8073135554792, -21.4153189228539));
            points.Add(new Vector2(37.9321386260801, -13.9911121942365));
            points.Add(new Vector2(35.9997481599954, -24.4324547954527));
            points.Add(new Vector2(42.6749485466956, -19.1016608693481));
            points.Add(new Vector2(32.3925070436908, -18.6774972052475));
            points.Add(new Vector2(42.6099189759439, -20.5204297749511));
            points.Add(new Vector2(37.1701796242022, -13.9762182164939));
            points.Add(new Vector2(32.8356931058067, -22.0679405960382));
            points.Add(new Vector2(34.7583987421143, -14.8209771462025));
            points.Add(new Vector2(35.2948330443942, -14.5752726487016));
            points.Add(new Vector2(32.4772409710376, -20.6948701925772));
            points.Add(new Vector2(33.2624426508598, -16.2887811536346));
            points.Add(new Vector2(41.4757653352503, -15.9811854347812));
            points.Add(new Vector2(41.5960474173068, -22.3194766792342));
            points.Add(new Vector2(42.6194922607308, -19.1338070425578));
            points.Add(new Vector2(40.2175325351175, -14.8183841380902));
            points.Add(new Vector2(34.5166755372144, -14.869853026341));
            points.Add(new Vector2(32.2913155308749, -18.8412971499142));
            points.Add(new Vector2(38.9632813761412, -24.131521660647));
            points.Add(new Vector2(34.5666590323511, -15.0683770404964));
            points.Add(new Vector2(34.5812400381482, -23.6272644572794));
            points.Add(new Vector2(40.6296863730894, -23.5465795128643));
            points.Add(new Vector2(32.5392994910186, -17.3680347938261));
            points.Add(new Vector2(40.2367257855246, -23.540219020791));
            points.Add(new Vector2(36.0250722033669, -24.3664036903953));
            points.Add(new Vector2(36.7628524534533, -14.1503396841912));
            points.Add(new Vector2(42.4411060766206, -17.9101411556001));
            points.Add(new Vector2(37.094159063976, -14.1772859824611));
            points.Add(new Vector2(36.8616041754395, -24.5003016828031));
            points.Add(new Vector2(32.7763428247867, -21.8958795344381));
            points.Add(new Vector2(38.6643105057406, -14.1522201149894));
            points.Add(new Vector2(39.1654798874077, -24.2312709507804));
            points.Add(new Vector2(37.7812967611793, -24.4693989191079));
            points.Add(new Vector2(42.3012331571046, -21.2155166454375));
            points.Add(new Vector2(32.295246020793, -20.5412742999433));
            points.Add(new Vector2(40.2890003645442, -23.6435413537927));
            points.Add(new Vector2(32.9082527156941, -16.7917235803535));
            points.Add(new Vector2(40.4146533286177, -15.0936311427148));
            points.Add(new Vector2(36.7753139636285, -24.5553231723713));
            points.Add(new Vector2(42.2363828870335, -17.7392212600201));
            points.Add(new Vector2(33.6922246666753, -15.517522975253));
            points.Add(new Vector2(34.1856121880184, -23.2213133449187));
            points.Add(new Vector2(33.7047238265948, -15.4822466738483));
            points.Add(new Vector2(39.2488212108923, -14.3696617938866));
            points.Add(new Vector2(33.0877953224963, -16.2282216475408));
            points.Add(new Vector2(42.3018488576488, -17.3473398318743));
            points.Add(new Vector2(36.0051067345885, -14.3189734739625));
            points.Add(new Vector2(38.8534013980404, -24.2939967683613));
            points.Add(new Vector2(42.2244528721257, -20.9825415819106));
            points.Add(new Vector2(37.3684441275185, -14.1561402485232));
            points.Add(new Vector2(42.5712727294698, -20.0377486130555));
            points.Add(new Vector2(41.9326732730089, -16.9615130273113));
            points.Add(new Vector2(41.8277069745106, -16.7951008043141));
            points.Add(new Vector2(42.3741895535338, -20.4070038103024));
            points.Add(new Vector2(42.4868277176991, -19.0002100808951));
            points.Add(new Vector2(42.1525567314397, -17.2042327233394));
            points.Add(new Vector2(36.2038162233059, -24.393249737972));
            points.Add(new Vector2(33.8085086751612, -23.1115564470895));
            points.Add(new Vector2(32.1701285054621, -18.4286314662948));
            points.Add(new Vector2(34.0790408476343, -15.3238464455733));
            points.Add(new Vector2(35.9499982013757, -14.2782810809874));
            points.Add(new Vector2(37.5350144248971, -14.0072400899886));
            points.Add(new Vector2(33.4849551693837, -22.4829351069143));
            points.Add(new Vector2(40.7519897911631, -15.1445937159273));
            points.Add(new Vector2(39.5056555928199, -23.9135620633464));
            points.Add(new Vector2(41.6151143305676, -22.1824235495852));
            points.Add(new Vector2(36.0216832755578, -24.348241935794));
            points.Add(new Vector2(34.6589918171874, -14.9622624770165));
            points.Add(new Vector2(37.2498775997674, -14.0111904794132));
            points.Add(new Vector2(37.680874162538, -24.5645141418875));
            points.Add(new Vector2(41.0839798013672, -23.0712188321122));
            points.Add(new Vector2(32.8298393662112, -16.6633500689981));
            points.Add(new Vector2(33.5333462475902, -22.947560414522));
            points.Add(new Vector2(33.5142792992879, -22.9683642270296));
            points.Add(new Vector2(33.2582904464175, -16.1337009636132));
            points.Add(new Vector2(38.3316635213029, -24.3963360991289));
            points.Add(new Vector2(34.8064773979305, -23.9548674778151));
            points.Add(new Vector2(39.4424106730033, -14.5404707800781));
            points.Add(new Vector2(37.69649111611, -24.4146895066514));
            points.Add(new Vector2(39.2696152216377, -24.0356404288756));
            points.Add(new Vector2(38.8081288774518, -14.142513796863));
            points.Add(new Vector2(41.0825001653756, -22.8047184297051));
            points.Add(new Vector2(36.8817118197476, -14.2078788968782));
            points.Add(new Vector2(32.3407300912813, -20.3335939093573));
            points.Add(new Vector2(42.5286771538164, -18.0757850506618));
            points.Add(new Vector2(35.0006375836775, -14.8501613825314));
            points.Add(new Vector2(35.956511207956, -14.3788230499986));
            points.Add(new Vector2(41.4283878094188, -22.5828415619752));
            points.Add(new Vector2(32.1399961593168, -19.9979509785617));
            points.Add(new Vector2(32.221999625638, -18.0839192184198));
            points.Add(new Vector2(39.6881461991885, -14.4955449635799));
            points.Add(new Vector2(34.7238680589795, -23.9201432116438));
            points.Add(new Vector2(38.1689981676821, -24.376944697309));
            points.Add(new Vector2(41.8484442212721, -21.9098291479338));
            points.Add(new Vector2(42.2404724647711, -17.2491881817899));
            points.Add(new Vector2(33.4842661796722, -22.5539511789085));
            points.Add(new Vector2(42.3927732611741, -18.5505846697167));
            points.Add(new Vector2(32.5690240884556, -17.0849951899636));
            points.Add(new Vector2(32.5436119040554, -17.175093281736));
            points.Add(new Vector2(37.3084252677279, -24.3789657718083));
            points.Add(new Vector2(40.7985334216933, -23.3500767640411));
            points.Add(new Vector2(42.1048728583154, -21.1648220420483));
            points.Add(new Vector2(33.2968000457702, -16.1631411997631));
            points.Add(new Vector2(34.2445587874437, -23.5528847652671));
            points.Add(new Vector2(42.1916191482273, -16.9965210925848));
            points.Add(new Vector2(36.9171751506937, -24.5308367899146));
            points.Add(new Vector2(39.8422369016618, -23.8544371711183));
            points.Add(new Vector2(36.8695981237316, -14.1862187279344));
            points.Add(new Vector2(32.9235739715656, -21.6279598753649));
            points.Add(new Vector2(36.4159959945253, -14.1681504863022));
            points.Add(new Vector2(33.9301195029761, -15.5431114811066));
            points.Add(new Vector2(33.9077306041374, -22.9877936212085));
            points.Add(new Vector2(38.0444989848343, -14.0745928886863));
            points.Add(new Vector2(35.1154349078932, -23.9221665518748));
            points.Add(new Vector2(34.7631439032094, -23.9010260908454));
            points.Add(new Vector2(40.9927982639231, -15.6572594865659));
            points.Add(new Vector2(33.4296840910368, -15.8711824261061));
            points.Add(new Vector2(37.9459448840213, -24.6234511779936));
            points.Add(new Vector2(36.9221483916294, -14.2393964393213));
            points.Add(new Vector2(32.8907353605668, -16.7053926264444));
            points.Add(new Vector2(41.705034243375, -16.6473184785325));
            points.Add(new Vector2(40.4694934295812, -23.5690895772192));
            points.Add(new Vector2(41.6247865094733, -16.2892891367338));
            points.Add(new Vector2(42.6434998520887, -20.3628984871732));
            points.Add(new Vector2(41.2041887946025, -22.7122414271715));
            points.Add(new Vector2(42.5919990633216, -18.1361899680006));
            points.Add(new Vector2(33.8545352983693, -22.9925757698347));
            points.Add(new Vector2(38.5986465813234, -14.2656457285821));
            points.Add(new Vector2(36.7167265253816, -14.217937620574));
            points.Add(new Vector2(38.5485263622142, -14.2932969713686));
            points.Add(new Vector2(33.0114696255457, -22.1814532928454));
            points.Add(new Vector2(34.6735945574028, -14.7034096783627));
            points.Add(new Vector2(37.8150262305819, -24.6075106754736));
            points.Add(new Vector2(34.5440687637755, -23.5005416825795));
            points.Add(new Vector2(33.4516289446322, -15.8835200702916));
            points.Add(new Vector2(35.1834081814452, -24.1368681062912));
            points.Add(new Vector2(32.8153855836421, -21.6131952088108));
            points.Add(new Vector2(38.1211728672333, -14.1957795593583));
            points.Add(new Vector2(42.1437701763479, -17.2026726499427));
            points.Add(new Vector2(42.2300329552169, -17.4389800121179));
            points.Add(new Vector2(38.2927038705399, -14.0900434521009));
            points.Add(new Vector2(40.7305509504098, -23.1164597754505));
            points.Add(new Vector2(32.7929590200448, -16.6526590421123));
            points.Add(new Vector2(32.9004130366719, -16.7155528219492));
            points.Add(new Vector2(32.420801086007, -20.4030679463359));
            points.Add(new Vector2(35.8643080325638, -24.173425601517));
            points.Add(new Vector2(33.7853890311961, -23.0501709389668));
            points.Add(new Vector2(32.6723930151191, -16.793968265377));
            points.Add(new Vector2(42.4096633776856, -20.7309594840897));
            points.Add(new Vector2(37.4980051380924, -14.085515149866));
            points.Add(new Vector2(36.643747332705, -24.2990031923317));
            points.Add(new Vector2(38.7132171950604, -24.4135925068106));
            points.Add(new Vector2(41.7747062241316, -21.8993750263705));
            points.Add(new Vector2(33.8676978671337, -23.1584346658399));
            points.Add(new Vector2(33.0070648470524, -21.8738202685364));
            points.Add(new Vector2(38.3629128090376, -24.5275996949172));
            points.Add(new Vector2(32.4472812448729, -17.5492715262041));
            points.Add(new Vector2(42.5895218460139, -20.1095271223375));
            points.Add(new Vector2(32.5608251568, -17.6683391207824));
            points.Add(new Vector2(33.4158023541591, -16.0212802869474));
            points.Add(new Vector2(34.3018811197637, -14.933086051393));
            points.Add(new Vector2(42.670235706782, -18.5698027919115));
            points.Add(new Vector2(40.2668523985191, -14.795717283856));
            points.Add(new Vector2(33.7788003680862, -15.3550623653073));
            points.Add(new Vector2(41.9208405741406, -16.8297996777983));
            points.Add(new Vector2(39.8704295369087, -23.9583596712352));
            points.Add(new Vector2(37.7584716998005, -24.6337222750679));
            points.Add(new Vector2(32.3827371052221, -17.9724429474198));
            points.Add(new Vector2(36.2236172483159, -14.1808110455429));
            points.Add(new Vector2(35.817280413355, -14.3337754683804));
            points.Add(new Vector2(32.2747536425315, -19.2449501232242));
            points.Add(new Vector2(32.2882785531292, -19.8916837333089));
            points.Add(new Vector2(41.4481570668967, -16.1681782246197));
            points.Add(new Vector2(33.114229403788, -16.4730301784241));
            points.Add(new Vector2(39.3732524113108, -24.2674472518146));
            points.Add(new Vector2(35.3124822582586, -24.0264320157357));
            points.Add(new Vector2(36.5699893371318, -14.1976038426892));
            points.Add(new Vector2(42.7144226508898, -19.951361860849));
            points.Add(new Vector2(32.7118628795777, -21.4451329315424));
            points.Add(new Vector2(37.9059659781685, -24.4373727873154));
            points.Add(new Vector2(32.8823501124261, -22.1112380698873));
            points.Add(new Vector2(32.2455640746659, -18.8332036325494));
            points.Add(new Vector2(33.9151444752068, -15.3113786525536));
            points.Add(new Vector2(32.1763419717174, -19.848879076024));
            points.Add(new Vector2(35.8381335021677, -24.2764548067758));
            points.Add(new Vector2(35.7611681040082, -24.219547634592));
            points.Add(new Vector2(41.3287824619891, -15.7432779540355));
            points.Add(new Vector2(42.2606529016683, -21.4190195253486));
            points.Add(new Vector2(40.7728393162188, -23.4584053360629));
            points.Add(new Vector2(35.0840910953142, -24.0821540346767));
            points.Add(new Vector2(38.5207295545097, -24.4763364929187));
            points.Add(new Vector2(37.2788327249436, -13.9904380377585));
            points.Add(new Vector2(32.2137520206932, -20.576900247662));
            points.Add(new Vector2(42.3184416898729, -21.2538384917953));
            points.Add(new Vector2(32.2996159660712, -17.8568830775025));
            points.Add(new Vector2(38.4609495141336, -24.4528875637151));
            points.Add(new Vector2(32.5766990330368, -21.514813578424));
            points.Add(new Vector2(37.5625714641041, -14.0358021869096));
            points.Add(new Vector2(33.5653006347332, -22.9467101167719));
            points.Add(new Vector2(37.6311636928129, -14.2382718432261));
            points.Add(new Vector2(36.7577810258315, -24.4338694588381));
            points.Add(new Vector2(41.6286614411158, -22.1149871570227));
            points.Add(new Vector2(40.9895525784274, -23.0760550270882));
            points.Add(new Vector2(37.9858219972628, -24.3611390586871));
            points.Add(new Vector2(33.2804604740537, -15.9455228030224));
            points.Add(new Vector2(42.6603236217182, -18.7482739213934));
            points.Add(new Vector2(40.1471293479497, -23.5601125153741));
            points.Add(new Vector2(38.4771860022623, -14.3341153907543));
            points.Add(new Vector2(39.165195598411, -14.4633302591745));
            points.Add(new Vector2(32.0880825442032, -19.5395064505785));
            points.Add(new Vector2(34.5038781593961, -23.663239001034));
            points.Add(new Vector2(42.1917469894746, -20.8942238948992));
            points.Add(new Vector2(34.8283347536106, -14.86015645646));
            points.Add(new Vector2(32.4885858842011, -17.5171169132788));
            points.Add(new Vector2(34.9290649957406, -23.8909438598317));
            points.Add(new Vector2(37.2813314582553, -24.4129106591785));
            points.Add(new Vector2(32.4742225702057, -17.7072285667181));
            points.Add(new Vector2(32.2410876355109, -18.2594105829501));
            points.Add(new Vector2(38.3337000075117, -24.5437768233075));
            points.Add(new Vector2(42.6678056336173, -19.4131752467213));
            points.Add(new Vector2(35.0820298859456, -24.109899348274));
            points.Add(new Vector2(38.3173987556039, -24.539500333302));
            points.Add(new Vector2(42.1587678630806, -16.9581513605668));
            points.Add(new Vector2(39.8844717258617, -23.8449310717461));
            points.Add(new Vector2(41.0615360532714, -23.0310302055453));
            points.Add(new Vector2(32.6648305646938, -21.6539516984499));
            points.Add(new Vector2(40.2812913412914, -15.1089650512068));
            points.Add(new Vector2(34.5523962967689, -15.1030103102422));
            points.Add(new Vector2(32.5430544941274, -20.9293308651604));
            points.Add(new Vector2(33.9359663133528, -15.2213711634456));
            points.Add(new Vector2(40.1592942840622, -23.7800191244129));
            points.Add(new Vector2(33.0118633910756, -22.1535790130249));
            points.Add(new Vector2(32.6525798691811, -21.7102051828855));
            points.Add(new Vector2(42.6581537947879, -18.436363547989));
            points.Add(new Vector2(32.5806825236393, -21.539896062521));
            points.Add(new Vector2(32.3465522336574, -17.9223513549587));
            points.Add(new Vector2(41.2933768644661, -16.0074047385635));
            points.Add(new Vector2(42.1413758425897, -17.138183356397));
            points.Add(new Vector2(35.3795520066376, -14.5260808175713));
            points.Add(new Vector2(41.0737543350171, -15.8170426714133));
            points.Add(new Vector2(41.7283112993527, -22.302659812112));
            points.Add(new Vector2(42.1038012936227, -21.2462926553486));
            points.Add(new Vector2(38.7702317659203, -14.2851024541966));
            points.Add(new Vector2(38.2542427010787, -14.1910047742322));
            points.Add(new Vector2(33.8825813527694, -15.4872432149082));
            points.Add(new Vector2(40.1042259001908, -23.5614368984163));
            points.Add(new Vector2(37.3759957157594, -24.5115799729158));
            points.Add(new Vector2(32.7091167980723, -16.8547637934529));
            points.Add(new Vector2(32.6743649681257, -21.7771642693319));
            points.Add(new Vector2(42.1386397316262, -21.1164494740896));
            points.Add(new Vector2(39.4067647955828, -24.061961709354));
            points.Add(new Vector2(32.1549623322412, -18.5055054163286));
            points.Add(new Vector2(33.5893415325791, -23.0253739509497));
            points.Add(new Vector2(33.7199761158347, -15.4907031496667));
            points.Add(new Vector2(36.8163706258169, -14.2066252158297));
            points.Add(new Vector2(32.2429095544375, -19.5672169713969));
            points.Add(new Vector2(32.6926504902992, -17.2219964759332));
            points.Add(new Vector2(42.2217467184844, -21.5040787021471));
            points.Add(new Vector2(32.3513161976983, -18.5560316201756));
            points.Add(new Vector2(34.0154301818397, -23.2495880228644));
            points.Add(new Vector2(32.2186731198236, -20.4457620569674));
            points.Add(new Vector2(41.9222595985027, -22.1600766702743));
            points.Add(new Vector2(32.282196533701, -18.1970434838566));
            points.Add(new Vector2(33.0249136631494, -22.1200907102455));
            points.Add(new Vector2(39.3812032282445, -23.9538538686918));
            points.Add(new Vector2(41.9878601251227, -21.672561791815));
            points.Add(new Vector2(42.3082445087991, -17.9558815916905));
            points.Add(new Vector2(32.272260249213, -18.9078989195896));
            points.Add(new Vector2(39.3670310682076, -24.1431838288316));
            points.Add(new Vector2(39.7406085733554, -14.6299700926705));
            points.Add(new Vector2(34.0069906270283, -23.1164866137446));
            points.Add(new Vector2(32.8374691698037, -17.1023262606592));
            points.Add(new Vector2(32.6606365227187, -21.2312273204542));
            points.Add(new Vector2(42.5288167864162, -17.7592146872539));
            points.Add(new Vector2(40.5779244331435, -15.1098657465641));
            points.Add(new Vector2(42.2953470140485, -17.299721511513));
            points.Add(new Vector2(32.3664825871667, -20.1922858667685));
            points.Add(new Vector2(32.8284130919318, -16.7563312705415));
            points.Add(new Vector2(33.3423630706258, -22.4854106155681));
            points.Add(new Vector2(38.2043324825937, -24.5557617908717));
            points.Add(new Vector2(34.4000358129722, -23.6511063318922));
            points.Add(new Vector2(34.3308951520201, -23.6154399191315));
            points.Add(new Vector2(41.0035288916046, -22.8568798224943));
            points.Add(new Vector2(34.8207962166878, -23.6770056492853));
            points.Add(new Vector2(42.1097298039717, -17.2349730058348));
            points.Add(new Vector2(33.0166803705991, -22.1768293037888));
            points.Add(new Vector2(38.0574404245187, -14.1799033424485));
            points.Add(new Vector2(42.332591979921, -20.5426151828822));
            points.Add(new Vector2(32.5122708221601, -17.9119896654693));
            points.Add(new Vector2(32.563645002805, -21.0461668587976));
            points.Add(new Vector2(42.1890876988851, -17.6411421252722));
            points.Add(new Vector2(42.6817797284162, -18.5682033544601));
            points.Add(new Vector2(35.1393790867748, -24.1250392862754));
            points.Add(new Vector2(34.4922118028327, -23.4421941238816));
            points.Add(new Vector2(32.2305731896555, -18.2201014636445));
            points.Add(new Vector2(33.3646439078648, -22.5245830471991));
            points.Add(new Vector2(33.2141255086169, -22.2890195490834));
            points.Add(new Vector2(33.2357419170973, -16.235014974683));
            points.Add(new Vector2(40.4563156269031, -15.2782226862487));
            points.Add(new Vector2(32.3728935440828, -20.6538835936528));
            points.Add(new Vector2(35.5353775953562, -24.164329569324));
            points.Add(new Vector2(42.6908228242275, -19.7576904405652));
            points.Add(new Vector2(41.3428464183185, -15.7906169772103));
            points.Add(new Vector2(32.546676071717, -17.7655328163045));
            points.Add(new Vector2(32.3486069307168, -18.7711531606212));
            points.Add(new Vector2(40.6245152736562, -15.4166914502244));
            points.Add(new Vector2(39.2291081296016, -24.2266366970993));
            points.Add(new Vector2(36.2860295406614, -24.497905327041));
            points.Add(new Vector2(35.7439948814261, -14.4133328584596));
            points.Add(new Vector2(42.3368775781971, -20.544701566651));
            points.Add(new Vector2(40.3739725836279, -14.8858830611386));
            points.Add(new Vector2(35.6665591716534, -14.5104163556668));
            points.Add(new Vector2(33.0915641678621, -16.5774835036262));
            points.Add(new Vector2(34.6750223020156, -23.688338504624));
            points.Add(new Vector2(42.5236229558802, -18.5037992404721));
            points.Add(new Vector2(34.0937195956134, -23.1762447720714));
            points.Add(new Vector2(42.5005624210449, -18.1340668483372));
            points.Add(new Vector2(33.2417840107068, -22.2994700846749));
            points.Add(new Vector2(36.517785326163, -24.4965326701742));
            points.Add(new Vector2(40.6863128856433, -15.3592099046249));
            points.Add(new Vector2(34.8260655532623, -14.7987120795028));
            points.Add(new Vector2(32.3663703106925, -19.7791932880269));
            points.Add(new Vector2(41.9791452659022, -21.8732470955619));
            points.Add(new Vector2(34.0591272668465, -23.4808282030613));
            points.Add(new Vector2(39.2553983220554, -24.2566285943859));
            points.Add(new Vector2(33.5498222285154, -15.8291685012445));
            points.Add(new Vector2(36.0434521638521, -24.3639239407391));
            points.Add(new Vector2(36.1369576837904, -14.1014132597678));
            points.Add(new Vector2(37.8780258838198, -14.0550788486176));
            points.Add(new Vector2(38.36561377024, -24.3255898877699));
            points.Add(new Vector2(40.7168950290871, -15.1163571288584));
            points.Add(new Vector2(41.8049566605434, -16.7417616544459));
            points.Add(new Vector2(33.407879704601, -22.6174278788109));
            points.Add(new Vector2(39.6998378733388, -14.7331981280352));
            points.Add(new Vector2(42.6364833250348, -20.0634512616159));
            points.Add(new Vector2(38.4304351173746, -24.3886050455521));
            points.Add(new Vector2(40.6392031491737, -15.3944337473356));
            points.Add(new Vector2(42.2056238293098, -17.5857172743971));
            points.Add(new Vector2(42.1032221426476, -21.6753497230831));
            points.Add(new Vector2(38.2896367475421, -14.2815428883532));
            points.Add(new Vector2(40.1628817809373, -23.8032294548241));
            points.Add(new Vector2(36.6138633527867, -14.2024519334389));
            points.Add(new Vector2(38.389800648954, -14.2736152985381));
            points.Add(new Vector2(39.163764926116, -24.182185570992));
            points.Add(new Vector2(36.7640907148561, -24.5806196880627));
            points.Add(new Vector2(32.5574738472167, -21.4554207412756));
            points.Add(new Vector2(36.5370231858113, -24.5540069025516));
            points.Add(new Vector2(37.4686719311453, -14.184564192299));
            points.Add(new Vector2(38.3919604162438, -14.2728182392413));
            points.Add(new Vector2(32.9398600828714, -21.973286135081));
            points.Add(new Vector2(32.9257460842463, -16.8741449150948));
            points.Add(new Vector2(36.4911918123939, -14.1618532363677));
            points.Add(new Vector2(33.4946300719392, -22.659641354659));
            points.Add(new Vector2(33.01385827967, -16.6835938147019));
            points.Add(new Vector2(32.1868811782986, -19.7851872957102));
            points.Add(new Vector2(32.6367620534049, -16.8598656359845));
            points.Add(new Vector2(33.3133508980211, -15.9053071114319));
            points.Add(new Vector2(42.0213470932704, -21.5588626355982));
            points.Add(new Vector2(42.3167540835199, -20.457724208977));
            points.Add(new Vector2(32.6769710294518, -21.2393715104916));
            points.Add(new Vector2(39.4118181236499, -14.4192487118483));
            points.Add(new Vector2(42.0950851791566, -21.5737507559312));
            points.Add(new Vector2(35.8586510019239, -24.1459574048155));
            points.Add(new Vector2(34.05100928916, -15.1919934006842));
            points.Add(new Vector2(42.0480768137906, -16.6997636957297));
            points.Add(new Vector2(32.4466751004351, -17.783918641439));
            points.Add(new Vector2(34.5586729993982, -15.0174825203389));
            points.Add(new Vector2(35.9776101285406, -14.3418202267113));
            points.Add(new Vector2(40.2357806051324, -14.9520516944484));
            points.Add(new Vector2(40.2772776318685, -14.8098089018747));
            points.Add(new Vector2(34.2371892772278, -23.5696569247475));
            points.Add(new Vector2(33.2304085386205, -16.0395678089294));
            points.Add(new Vector2(42.1444768952623, -21.2599272835234));
            points.Add(new Vector2(39.0024195812851, -14.2384818061422));
            points.Add(new Vector2(32.1395811806951, -20.0617483305996));
            points.Add(new Vector2(34.4661682202376, -14.894578820944));
            points.Add(new Vector2(34.585897463618, -23.5609164329105));
            points.Add(new Vector2(32.2101629537514, -19.0729500863049));
            points.Add(new Vector2(40.3524279787545, -23.5857495062804));
            points.Add(new Vector2(40.8548441194049, -15.5261548314748));
            points.Add(new Vector2(42.502685630382, -20.6973950489081));
            points.Add(new Vector2(38.2161439754941, -24.2930676852946));
            points.Add(new Vector2(41.6927609251437, -22.3716143735892));
            points.Add(new Vector2(41.5187177312096, -22.6603983946331));
            points.Add(new Vector2(42.5504668518297, -19.219998872963));
            points.Add(new Vector2(33.4763737850371, -15.9311574836345));
            points.Add(new Vector2(35.494168988481, -24.0869057701883));
            points.Add(new Vector2(42.1521258964033, -16.9136908296865));
            points.Add(new Vector2(32.5582125312674, -21.1975924560243));
            points.Add(new Vector2(40.6956951421875, -15.1837836979932));
            points.Add(new Vector2(41.9492290930376, -22.1164389454395));
            points.Add(new Vector2(34.1579583675684, -23.2074425307759));
            points.Add(new Vector2(42.0356610832448, -16.9476358759567));
            points.Add(new Vector2(37.8920850749655, -14.1416519250922));
            points.Add(new Vector2(41.5107209932932, -22.5284108646086));
            points.Add(new Vector2(33.2545402749178, -16.0366064730291));
            points.Add(new Vector2(36.6130922568662, -14.2039389811413));
            points.Add(new Vector2(32.6255984103126, -21.0778053507224));
            points.Add(new Vector2(37.5299443947421, -14.0883944010863));
            points.Add(new Vector2(40.5550481112866, -15.2443742923711));
            points.Add(new Vector2(35.202563226168, -14.7423860296064));
            points.Add(new Vector2(34.3338547422664, -15.2796867363858));
            points.Add(new Vector2(32.1400860576364, -20.1836029555076));
            points.Add(new Vector2(39.9192881936194, -23.8627835509539));
            points.Add(new Vector2(42.315977425731, -17.3022832344096));
            points.Add(new Vector2(33.5580456651268, -22.6269404617685));
            points.Add(new Vector2(35.8063689299306, -24.2316435913945));
            points.Add(new Vector2(36.7239775058201, -14.2127897199021));
            points.Add(new Vector2(33.3662392633523, -15.8015098300741));
            points.Add(new Vector2(41.5540843029823, -16.3528654001925));
            points.Add(new Vector2(34.8818916336909, -23.7390900327125));
            points.Add(new Vector2(36.0021745510776, -24.3851410556536));
            points.Add(new Vector2(37.4161974438757, -14.0227484901241));
            points.Add(new Vector2(32.77084666784, -16.9094734609144));
            points.Add(new Vector2(34.2210980102187, -23.5879404698291));
            points.Add(new Vector2(38.2081611736535, -24.3661914185935));
            points.Add(new Vector2(34.0184499506471, -15.3385452716751));
            points.Add(new Vector2(41.6800367689085, -16.2548632844733));
            points.Add(new Vector2(35.7572487619261, -24.0959851316993));
            points.Add(new Vector2(41.915609459249, -16.8729835943897));
            points.Add(new Vector2(41.448044358433, -16.216378205158));
            points.Add(new Vector2(41.9318871449351, -21.9494475129361));
            points.Add(new Vector2(40.7789284482586, -15.2020344922236));
            points.Add(new Vector2(42.716823381845, -18.8780693739332));
            points.Add(new Vector2(34.9513234156761, -23.8959400289));
            points.Add(new Vector2(33.7079478351976, -22.9121922566569));
            points.Add(new Vector2(38.8707362193228, -14.4449494431806));
            points.Add(new Vector2(42.6996286123653, -19.4138589419298));
            points.Add(new Vector2(42.5413738872984, -18.7687221432114));
            points.Add(new Vector2(37.1151295840129, -13.9608491129896));
            points.Add(new Vector2(33.7823063479411, -22.8340154762997));
            points.Add(new Vector2(38.0514730777638, -24.5905318435076));
            points.Add(new Vector2(32.7843771762037, -21.468947352205));
            points.Add(new Vector2(42.1918223973967, -17.0951738701068));
            points.Add(new Vector2(32.2224955975469, -19.8440294554395));
            points.Add(new Vector2(41.8391566118566, -22.1989340915554));
            points.Add(new Vector2(35.0745363448957, -14.693282058626));
            points.Add(new Vector2(32.8946757993034, -16.6382031692616));
            points.Add(new Vector2(36.0567950190414, -14.1273699849787));
            points.Add(new Vector2(38.361964781262, -14.2779565365255));
            points.Add(new Vector2(36.6765448214758, -14.1581771919865));
            points.Add(new Vector2(32.8373836299072, -21.478315989582));
            points.Add(new Vector2(41.0660001793419, -23.0341465097817));
            points.Add(new Vector2(39.9101691769041, -14.5798936841793));
            points.Add(new Vector2(37.3878672967131, -24.5637513982183));
            points.Add(new Vector2(39.5309381454611, -24.0336857967288));
            points.Add(new Vector2(42.2747681582996, -17.5862986263933));
            points.Add(new Vector2(42.7461897171057, -19.0466063184486));
            points.Add(new Vector2(36.1320795170837, -24.3453202971368));
            points.Add(new Vector2(37.2364465782823, -24.4670957740042));
            points.Add(new Vector2(39.0849437163638, -14.2773419519868));
            points.Add(new Vector2(36.0107453218979, -24.282859123604));
            points.Add(new Vector2(32.3970088367304, -18.2077247951523));
            points.Add(new Vector2(40.0726324088056, -14.6695074478411));
            points.Add(new Vector2(40.6436288558198, -15.3274812824163));
            points.Add(new Vector2(32.5601834924941, -17.8369106304909));
            points.Add(new Vector2(41.9423455108978, -22.1145875158474));
            points.Add(new Vector2(32.4950206577807, -20.8166985695239));
            points.Add(new Vector2(32.3122471942887, -19.7941915774952));
            points.Add(new Vector2(37.600715127316, -24.5318187472144));
            points.Add(new Vector2(38.6963720763584, -24.3722131618514));
            points.Add(new Vector2(34.1173926245654, -15.3327993407222));
            points.Add(new Vector2(35.2144704514267, -14.5380813267153));
            points.Add(new Vector2(41.2198674767639, -22.5963493774664));
            points.Add(new Vector2(42.6727072219786, -19.3242928293305));
            points.Add(new Vector2(36.8657184876858, -14.0805523078033));
            points.Add(new Vector2(42.4210947887113, -19.8500340720576));
            points.Add(new Vector2(38.6089265924531, -24.4388366186813));
            points.Add(new Vector2(32.0890874002132, -19.2419374229295));
            points.Add(new Vector2(32.3286871265017, -20.6168882252319));
            points.Add(new Vector2(37.0362891509437, -24.5456585054532));
            points.Add(new Vector2(41.4362044025349, -16.236390127917));
            points.Add(new Vector2(39.7860161440487, -23.9797649328855));
            points.Add(new Vector2(33.8189606811742, -23.1652875696358));
            points.Add(new Vector2(32.5711469103781, -20.973654653242));
            points.Add(new Vector2(32.1081506034724, -19.5152205968913));
            points.Add(new Vector2(37.9993116972903, -24.3921765099651));
            points.Add(new Vector2(36.6608388553828, -14.2162297457628));
            points.Add(new Vector2(33.0509056994926, -16.7011684798122));
            points.Add(new Vector2(42.275485441682, -20.9919552718973));
            points.Add(new Vector2(38.3172605891117, -14.2053280220533));
            points.Add(new Vector2(42.2786428114349, -17.5607240656122));
            points.Add(new Vector2(37.097413230064, -14.1655993962328));
            points.Add(new Vector2(42.512008095394, -19.016427698346));
            points.Add(new Vector2(37.2660250037684, -24.4355918690221));
            points.Add(new Vector2(40.3283896461288, -23.5771356894975));
            points.Add(new Vector2(35.44521189042, -14.606178887697));
            points.Add(new Vector2(42.7214358302264, -18.7727937777938));
            points.Add(new Vector2(41.2179707736348, -22.8087478973731));
            points.Add(new Vector2(42.5786785022489, -19.4987246064898));
            points.Add(new Vector2(41.0598492455178, -15.6917213029063));
            points.Add(new Vector2(42.3602520378365, -17.9314095622969));
            points.Add(new Vector2(36.8635669824506, -24.5106028827596));
            points.Add(new Vector2(35.610115415467, -24.2306811619382));
            points.Add(new Vector2(41.8904402960576, -16.7436737749593));
            points.Add(new Vector2(32.1989732271544, -19.1543447396388));
            points.Add(new Vector2(40.6806211262058, -23.180758151301));
            points.Add(new Vector2(32.4797107976527, -17.3498107058781));
            points.Add(new Vector2(35.6020762752793, -24.2145975837083));
            points.Add(new Vector2(33.1669192550109, -16.5182569981828));
            points.Add(new Vector2(42.5241415852671, -18.9437478158053));
            points.Add(new Vector2(42.3768095288414, -20.71951103942));
            points.Add(new Vector2(33.3213873837866, -16.2458878030876));
            points.Add(new Vector2(41.2666299600643, -15.8464770598436));
            points.Add(new Vector2(36.1018379478476, -24.4782576532047));
            points.Add(new Vector2(32.3797956922531, -20.2556055221314));
            points.Add(new Vector2(38.8946409377061, -14.3999199252643));
            points.Add(new Vector2(32.1952807407692, -18.3433295832753));
            points.Add(new Vector2(36.6526793282836, -14.2884093942563));
            points.Add(new Vector2(40.6834563833382, -15.3286949367256));
            points.Add(new Vector2(42.0670625577632, -21.3827292452849));
            points.Add(new Vector2(37.9727210138213, -13.986918832916));
            points.Add(new Vector2(38.225298457037, -14.0225278499049));
            points.Add(new Vector2(36.197924531969, -14.0827572310972));
            points.Add(new Vector2(34.3290330374167, -15.2379415713224));
            points.Add(new Vector2(39.4768392016795, -24.009684446365));
            points.Add(new Vector2(35.7584837929367, -24.3401627469016));
            points.Add(new Vector2(42.2416500287882, -21.4251134244112));
            points.Add(new Vector2(42.4773794209464, -20.5863399168171));
            points.Add(new Vector2(42.4495040342323, -18.1681308721382));
            points.Add(new Vector2(39.7587971497947, -23.8230177166388));
            points.Add(new Vector2(32.2282542849513, -18.599599185303));
            points.Add(new Vector2(36.6878338411971, -24.4472049939226));
            points.Add(new Vector2(42.69327455203, -19.0206237636181));
            points.Add(new Vector2(34.2437777532963, -23.2668811171478));
            points.Add(new Vector2(40.9700769284723, -15.6552199506172));
            points.Add(new Vector2(32.6486728065442, -21.5829723066843));
            points.Add(new Vector2(41.1230095435088, -22.9467320740325));
            points.Add(new Vector2(32.6994309935416, -21.3577725063549));
            points.Add(new Vector2(33.4589736814938, -22.7472621589053));
            points.Add(new Vector2(39.785517152008, -23.82125862242));
            points.Add(new Vector2(32.307973758664, -19.0420009449885));
            points.Add(new Vector2(42.3812101823615, -21.0254354173269));
            points.Add(new Vector2(34.6323436285619, -23.5648870663784));
            points.Add(new Vector2(32.347760158256, -20.0900557128177));
            points.Add(new Vector2(36.8826342278003, -24.353931419725));
            points.Add(new Vector2(37.4745969643785, -24.4199522008052));
            points.Add(new Vector2(42.4709743349574, -17.6538469152157));
            points.Add(new Vector2(36.5898785767835, -24.5130669164925));
            points.Add(new Vector2(33.449759324191, -22.4964803288436));
            points.Add(new Vector2(41.4285960463747, -22.7309046931238));
            points.Add(new Vector2(38.9970226005927, -24.3681954088735));
            points.Add(new Vector2(36.4425522610369, -24.3613920464426));
            points.Add(new Vector2(38.591643013265, -24.3393530550087));
            points.Add(new Vector2(38.7302045847794, -14.3381680541655));
            points.Add(new Vector2(38.0649107218635, -14.0741370917046));
            points.Add(new Vector2(34.0878917098343, -15.2260817908276));
            points.Add(new Vector2(37.1008163790688, -24.6192759312959));
            points.Add(new Vector2(37.0836108807464, -24.4495376816178));
            points.Add(new Vector2(32.4842049006441, -17.5453391897524));
            points.Add(new Vector2(35.9635641003235, -14.395905363875));
            points.Add(new Vector2(40.9489871216305, -15.6178944666145));
            points.Add(new Vector2(40.6098629303714, -15.0660685152789));
            points.Add(new Vector2(42.4216447076132, -17.67198887719));
            points.Add(new Vector2(32.5945389635365, -17.176483949646));
            points.Add(new Vector2(32.9001653594454, -21.9841935162402));
            points.Add(new Vector2(42.5573412436977, -20.3053413671055));
            points.Add(new Vector2(32.5775213962598, -20.9332148009967));
            points.Add(new Vector2(34.4358673892092, -15.1066291140856));
            points.Add(new Vector2(33.0799440152355, -22.1069498609758));
            points.Add(new Vector2(35.040275537398, -23.9968921832135));
            points.Add(new Vector2(34.3856636371154, -14.8937336379634));
            points.Add(new Vector2(42.4958966425128, -20.4648950634311));
            points.Add(new Vector2(32.5014209583123, -20.6677466177097));
            points.Add(new Vector2(42.2975977960861, -17.8297006703326));
            points.Add(new Vector2(35.0510902700114, -24.0431875938412));
            points.Add(new Vector2(36.2585052030345, -14.0762733318675));
            points.Add(new Vector2(36.1865500330877, -24.5161730997289));
            points.Add(new Vector2(41.9519323555429, -21.5098143779501));
            points.Add(new Vector2(32.15113334363, -19.4620770875887));
            points.Add(new Vector2(32.7039563348929, -17.0800254403421));
            points.Add(new Vector2(37.679314690145, -14.0652377532443));
            points.Add(new Vector2(41.3296458634431, -22.525008100034));
            points.Add(new Vector2(34.5301340296647, -23.8204639658866));
            points.Add(new Vector2(38.7151310849237, -24.2550622121555));
            points.Add(new Vector2(39.3680944427748, -24.1538088718035));
            points.Add(new Vector2(39.3822387247995, -24.0037624573997));
            points.Add(new Vector2(38.7839434389776, -14.2387364724802));
            points.Add(new Vector2(32.4517316069918, -21.3163532835847));
            points.Add(new Vector2(39.7374161434515, -14.527176318863));
            points.Add(new Vector2(42.1856827992838, -21.3514833699279));
            points.Add(new Vector2(32.8457702036521, -16.9259976828267));
            points.Add(new Vector2(32.5878080262481, -17.2076105593489));
            points.Add(new Vector2(39.2443618343014, -14.4746498881963));
            points.Add(new Vector2(33.1476432605399, -16.1814719238304));
            points.Add(new Vector2(36.1926711590859, -14.2291202389549));
            points.Add(new Vector2(37.4939790080699, -24.5146161037538));
            points.Add(new Vector2(32.0837867802738, -18.9170617927934));
            points.Add(new Vector2(39.5723387908299, -23.8930762093419));
            points.Add(new Vector2(41.5967868121912, -22.447646473396));
            points.Add(new Vector2(41.1129257176705, -22.728002836128));
            points.Add(new Vector2(32.2725405197316, -18.7761075706555));
            points.Add(new Vector2(41.8009886320934, -21.9654856435346));
            points.Add(new Vector2(36.4663845737624, -24.2666871652004));
            points.Add(new Vector2(41.6559195489265, -16.4633655102703));
            points.Add(new Vector2(40.3823251760828, -23.6220489028152));
            points.Add(new Vector2(35.9562774083642, -24.4174783014465));
            points.Add(new Vector2(42.058336134093, -17.0737755850075));
            points.Add(new Vector2(37.2954478118802, -24.5920302944968));
            points.Add(new Vector2(41.6362257249178, -16.4623924487362));
            points.Add(new Vector2(33.7937204944382, -15.6097568967786));
            points.Add(new Vector2(37.0608647587084, -24.5406065986012));
            points.Add(new Vector2(42.3665492366845, -18.3299006845049));
            points.Add(new Vector2(32.1544510319175, -18.4889578781674));
            points.Add(new Vector2(38.4549692611242, -14.1108431826811));
            points.Add(new Vector2(40.0401274090971, -14.7136449371201));
            points.Add(new Vector2(42.0033242517802, -17.1904565077613));
            points.Add(new Vector2(38.6847930672562, -24.3470731255859));
            points.Add(new Vector2(40.252433106442, -23.7988009106688));
            points.Add(new Vector2(42.0071062745862, -17.067795451709));
            points.Add(new Vector2(34.3569079875818, -23.6884029100082));
            points.Add(new Vector2(35.7512251454986, -14.2864774574021));
            points.Add(new Vector2(42.7114372414523, -18.9450508658621));
            points.Add(new Vector2(42.2461365031594, -21.081319195253));
            points.Add(new Vector2(33.0288680389799, -16.2202335172001));
            points.Add(new Vector2(33.1918379596362, -16.4764963055037));
            points.Add(new Vector2(35.872250367641, -14.305059453292));
            points.Add(new Vector2(35.7260011042989, -14.2619131061091));
            points.Add(new Vector2(33.2400439838816, -22.4675933873216));
            points.Add(new Vector2(38.610180333805, -14.2344498659988));
            points.Add(new Vector2(42.3320697083662, -20.5737083600836));
            points.Add(new Vector2(41.1919625824398, -15.660534183129));
            points.Add(new Vector2(32.5221796818935, -20.7765915684785));
            points.Add(new Vector2(35.1521455594707, -23.8610958975097));
            points.Add(new Vector2(40.3622895989604, -14.9340034667913));
            points.Add(new Vector2(34.1414865073065, -15.2365920282104));
            points.Add(new Vector2(33.41262560459, -22.6018678284637));
            points.Add(new Vector2(32.4292151829873, -18.2207461273326));
            points.Add(new Vector2(42.6546610216381, -19.9860093369457));
            points.Add(new Vector2(42.1974524557833, -21.4918614850003));
            points.Add(new Vector2(41.3644523451298, -22.6047834973391));
            points.Add(new Vector2(39.2249221877462, -14.3026537488793));
            points.Add(new Vector2(33.5799645950587, -15.660215387831));
            points.Add(new Vector2(40.9568576115597, -15.4843410643808));
            points.Add(new Vector2(34.2785943309245, -23.5820404702285));
            points.Add(new Vector2(39.1606754328323, -14.2700634765062));
            points.Add(new Vector2(32.3185513917894, -18.0674595535385));
            points.Add(new Vector2(38.7687092523575, -24.2275769276347));
            points.Add(new Vector2(33.699848824324, -15.8241910191134));
            points.Add(new Vector2(32.2808590658475, -18.8779351934933));
            points.Add(new Vector2(37.6266074110963, -14.1874756613351));
            points.Add(new Vector2(38.7836132727983, -14.337324078813));
            points.Add(new Vector2(36.830240328707, -24.4106763640761));
            points.Add(new Vector2(42.226800234206, -17.1593211667839));
            points.Add(new Vector2(34.4994199961592, -15.1221986414952));
            points.Add(new Vector2(41.2519731286217, -15.9772691673539));
            points.Add(new Vector2(41.0012937489936, -15.5564494782241));
            points.Add(new Vector2(38.0461411056873, -14.1104296724434));
            points.Add(new Vector2(36.0938705103412, -24.3939327847528));
            points.Add(new Vector2(36.8725019707979, -24.3198551167766));
            points.Add(new Vector2(35.5255365688203, -14.5996775810707));
            points.Add(new Vector2(42.6134586205019, -18.1492149722805));
            points.Add(new Vector2(39.2614754479451, -24.1814246781545));
            points.Add(new Vector2(38.5447945340516, -14.335778747907));
            points.Add(new Vector2(33.801663343308, -22.9288001337549));
            points.Add(new Vector2(32.8260615786051, -21.7133199500693));
            points.Add(new Vector2(32.7732608812768, -17.2893021295475));
            points.Add(new Vector2(33.072183087559, -16.4214169511149));
            points.Add(new Vector2(35.3724959612426, -14.5926194077591));
            points.Add(new Vector2(37.4070208522419, -14.0744395309179));
            points.Add(new Vector2(32.8467140400931, -21.619337765759));
            points.Add(new Vector2(33.5414219200692, -15.7512220742683));
            points.Add(new Vector2(33.0572250231833, -22.2477249457514));
            points.Add(new Vector2(33.5045823748954, -22.7537860334243));
            points.Add(new Vector2(33.400561416333, -16.1028264665666));
            points.Add(new Vector2(32.9616707343111, -21.9256291333109));
            points.Add(new Vector2(39.5012397362416, -24.1992852958133));
            points.Add(new Vector2(42.1316679419676, -17.0264583417214));
            points.Add(new Vector2(40.9378722936475, -23.0174049405711));
            points.Add(new Vector2(42.2990475224259, -17.9128491970084));
            points.Add(new Vector2(32.497996810146, -18.0631051695492));
            points.Add(new Vector2(42.2598484883228, -20.9075716285683));
            points.Add(new Vector2(40.8615625811935, -15.4153928100253));
            points.Add(new Vector2(32.3621732677783, -20.1820940502801));
            points.Add(new Vector2(35.7233458670919, -14.3710625724358));
            points.Add(new Vector2(38.6243604460125, -14.1663848713581));
            points.Add(new Vector2(42.6913870571809, -19.9423602640557));
            points.Add(new Vector2(42.19099949773, -17.0620688779059));
            points.Add(new Vector2(32.3913371775142, -17.60798221072));
            points.Add(new Vector2(33.0951241625717, -16.4283339937103));
            points.Add(new Vector2(34.638659576569, -23.8572960014038));
            points.Add(new Vector2(37.6359196175746, -24.57749974825));
            points.Add(new Vector2(33.1560826859878, -22.5495606694087));
            points.Add(new Vector2(42.5295984883111, -18.9107383638014));
            points.Add(new Vector2(32.6423725737802, -17.4605745139326));
            points.Add(new Vector2(39.2099857531685, -14.2869484042066));
            points.Add(new Vector2(35.5649284569772, -14.2837769012416));
            points.Add(new Vector2(40.8905012269185, -22.9718572627059));
            points.Add(new Vector2(42.3148686345233, -21.1764404512827));
            points.Add(new Vector2(32.4708382580002, -20.8469875900559));
            points.Add(new Vector2(32.9031560960194, -16.6362292555996));
            points.Add(new Vector2(39.8098483845939, -23.9732372059439));
            points.Add(new Vector2(40.6315147656175, -23.2732523794857));
            points.Add(new Vector2(32.6856028279302, -21.669612065969));
            points.Add(new Vector2(34.3488277914824, -23.6535439634875));
            points.Add(new Vector2(37.7413722864808, -14.0712844919414));
            points.Add(new Vector2(42.5569874451104, -18.7030628420173));
            points.Add(new Vector2(32.3672755907748, -20.6502229077157));
            points.Add(new Vector2(32.4390106781958, -17.983260561803));
            points.Add(new Vector2(32.4247015995004, -20.8698550891029));
            points.Add(new Vector2(34.3224791466728, -15.0318681739039));
            points.Add(new Vector2(32.4459352744734, -17.8787693009055));
            points.Add(new Vector2(42.4443502154813, -20.8948783641957));
            points.Add(new Vector2(32.1020266244321, -19.4259779055481));
            points.Add(new Vector2(38.4219056987546, -24.4273008682652));
            points.Add(new Vector2(32.2219843783817, -18.6764535978562));
            points.Add(new Vector2(36.6218100530788, -14.0607389045238));
            points.Add(new Vector2(37.1361252412524, -14.1885771798457));
            points.Add(new Vector2(33.217975135378, -22.6221197464551));
            points.Add(new Vector2(42.5474302917974, -19.8868582692771));
            points.Add(new Vector2(41.5651958958364, -16.0674065313796));
            points.Add(new Vector2(42.3130095763403, -20.6636974088654));
            points.Add(new Vector2(34.6242933941917, -14.8603650102503));
            points.Add(new Vector2(41.9395626294284, -16.9236868995288));
            points.Add(new Vector2(33.7198753300943, -22.8158743754302));
            points.Add(new Vector2(42.5614142684046, -19.8123796215362));
            points.Add(new Vector2(32.9697391140766, -22.2052072551714));
            points.Add(new Vector2(41.3047024111388, -15.7868147198065));
            points.Add(new Vector2(35.2264226225937, -24.0095825937575));
            points.Add(new Vector2(32.3754421441903, -20.1361428925908));
            points.Add(new Vector2(42.6860508652234, -18.5438368915894));
            points.Add(new Vector2(42.4716158618996, -19.4537241232109));
            points.Add(new Vector2(32.4024160505723, -20.0589449154813));
            points.Add(new Vector2(33.1852414117028, -22.4978470652604));
            points.Add(new Vector2(40.4463576796941, -23.3797149491884));
            points.Add(new Vector2(35.5932655229982, -24.2240450411614));
            points.Add(new Vector2(37.6717260738604, -24.5056146167941));
            points.Add(new Vector2(38.0666031734025, -14.0122553409144));
            points.Add(new Vector2(42.5936087431745, -18.779423379851));
            points.Add(new Vector2(42.6403279081278, -20.2767401683065));
            points.Add(new Vector2(42.0284965284968, -21.7501009253173));
            points.Add(new Vector2(40.716753849918, -23.3988869541545));
            points.Add(new Vector2(32.3199436253568, -17.9266021292654));
            points.Add(new Vector2(33.1107221198546, -22.2415458202549));
            points.Add(new Vector2(33.1949453802196, -16.2025009122781));
            points.Add(new Vector2(37.7127317660338, -24.5383663363869));
            points.Add(new Vector2(33.2728706589467, -22.6487929469652));
            points.Add(new Vector2(42.3654919052293, -17.8744747764721));
            points.Add(new Vector2(42.3714193713041, -20.4816686950372));
            points.Add(new Vector2(32.2337140466544, -19.3960877106574));
            points.Add(new Vector2(32.6277741776682, -21.715897250456));
            points.Add(new Vector2(42.2297826056163, -17.7013746993873));
            points.Add(new Vector2(37.3132342460996, -24.4994284399498));
            points.Add(new Vector2(39.4576333641307, -14.4743867634399));
            points.Add(new Vector2(42.0877630692205, -16.9110148571031));
            points.Add(new Vector2(41.9439238390087, -21.5145459632013));
            points.Add(new Vector2(41.6340854157427, -22.1503422096157));
            points.Add(new Vector2(32.6285710971576, -21.6016467494266));
            points.Add(new Vector2(34.3766034754529, -23.471065254322));
            points.Add(new Vector2(42.0765576585155, -17.3671091566901));
            points.Add(new Vector2(37.4650960042507, -24.3682807075463));
            points.Add(new Vector2(38.798064845637, -24.4284719099445));
            points.Add(new Vector2(39.6349142754799, -14.6327011717686));
            points.Add(new Vector2(32.0903921990123, -18.9758270420699));
            points.Add(new Vector2(34.7429038735112, -23.597386106402));
            points.Add(new Vector2(38.1688849279191, -24.5670207465879));
            points.Add(new Vector2(37.9378298561831, -14.2284806776435));
            points.Add(new Vector2(42.5107257089051, -20.7017040100107));
            points.Add(new Vector2(32.2325500206928, -19.7419855642468));
            points.Add(new Vector2(42.4213405686367, -18.2991921239139));
            points.Add(new Vector2(35.7750249911929, -14.4652854972471));
            points.Add(new Vector2(33.708517938914, -15.5196714485654));
            points.Add(new Vector2(42.3408347973465, -20.8278287471717));
            points.Add(new Vector2(42.7370368464647, -18.925861376189));
            points.Add(new Vector2(33.0310484258817, -16.6722201862246));
            points.Add(new Vector2(42.1067293689069, -17.3260470195885));
            points.Add(new Vector2(37.163522703459, -13.9875084075602));
            points.Add(new Vector2(33.309803530791, -22.5198539640185));
            points.Add(new Vector2(39.1041608672124, -14.4642486883552));
            points.Add(new Vector2(36.6735220192803, -24.3645335816923));
            points.Add(new Vector2(41.7413207314006, -16.4196504399633));
            points.Add(new Vector2(42.3787649147674, -18.406571025856));
            points.Add(new Vector2(41.4737737471906, -22.6629471578314));
            points.Add(new Vector2(32.1060923291697, -19.5661643097895));
            points.Add(new Vector2(42.111437491992, -21.5151208311766));
            points.Add(new Vector2(35.1016671730066, -24.0598127994683));
            points.Add(new Vector2(33.91925809391, -15.3998753200097));
            points.Add(new Vector2(41.8781456566042, -16.9394024209352));
            points.Add(new Vector2(33.0765341586499, -16.4207223088935));
            points.Add(new Vector2(39.746304643056, -14.6586656358922));
            points.Add(new Vector2(34.5787560213945, -23.7344755561645));
            points.Add(new Vector2(40.5639823877716, -15.1315696212838));
            points.Add(new Vector2(32.7832147918119, -16.942708833872));
            points.Add(new Vector2(39.0179211772105, -14.3387018219753));
            points.Add(new Vector2(42.0559768088077, -17.1305749421648));
            points.Add(new Vector2(35.5403021786486, -14.4650913063897));
            points.Add(new Vector2(37.2501496485691, -14.0207747539154));
            points.Add(new Vector2(35.5605574000437, -14.3860362482872));
            points.Add(new Vector2(34.6987481109998, -14.8146967971855));
            points.Add(new Vector2(33.388350995111, -22.6860072679793));
            points.Add(new Vector2(32.6896741971178, -16.9561141759762));
            points.Add(new Vector2(33.8300858365265, -15.462945098184));
            points.Add(new Vector2(42.3314963753274, -20.9722876952155));
            points.Add(new Vector2(38.4362646822265, -24.4734657638793));
            points.Add(new Vector2(35.7690308737192, -24.3771183897718));
            points.Add(new Vector2(39.0135089273147, -24.3229030573819));
            points.Add(new Vector2(41.666217089399, -16.5858270745604));
            points.Add(new Vector2(32.086400940029, -18.9059826568718));
            points.Add(new Vector2(41.3011568102735, -22.7269938473033));
            points.Add(new Vector2(38.5277685261783, -14.2733370353313));
            points.Add(new Vector2(36.4215482552584, -14.2752932406129));
            points.Add(new Vector2(32.5616331847633, -21.5668259659427));
            points.Add(new Vector2(38.7991078794689, -24.2631279445245));
            points.Add(new Vector2(35.6717122000222, -24.2617637639486));
            points.Add(new Vector2(38.4807169645992, -14.3362176260091));
            points.Add(new Vector2(33.5674819340544, -22.7362709882921));
            points.Add(new Vector2(41.3171754999974, -15.8762328024531));
            points.Add(new Vector2(35.7462548163143, -14.3196777450863));
            points.Add(new Vector2(40.7241314415523, -23.1972359568479));
            points.Add(new Vector2(39.7277082422762, -24.0163770181469));
            points.Add(new Vector2(32.3440201421905, -18.642900399153));
            points.Add(new Vector2(42.4925871992062, -19.973790415349));
            points.Add(new Vector2(35.0693743018331, -14.5640219223839));
            points.Add(new Vector2(32.1378493469971, -19.8060393162137));
            points.Add(new Vector2(32.2225925692709, -19.7038607753728));
            points.Add(new Vector2(32.5022502683481, -21.2542774251523));
            points.Add(new Vector2(32.2697527114901, -19.5796369755765));
            points.Add(new Vector2(37.2168312923631, -13.9949019066516));
            points.Add(new Vector2(40.7988950593748, -15.4827728065917));
            points.Add(new Vector2(42.3839079758532, -20.6361441537461));
            points.Add(new Vector2(40.4597633576977, -14.9889257112111));
            points.Add(new Vector2(42.1626311646997, -21.3578373899057));
            points.Add(new Vector2(41.8478318249006, -22.0654751966211));
            points.Add(new Vector2(37.3685765430965, -14.1777250474423));
            points.Add(new Vector2(39.4797307121054, -14.5960470702827));
            points.Add(new Vector2(37.7175673592172, -24.6193642115614));
            points.Add(new Vector2(36.6363862364915, -24.3542143324244));
            points.Add(new Vector2(38.8701297725533, -24.1632222360635));
            points.Add(new Vector2(42.4932998604418, -18.7657921144686));
            points.Add(new Vector2(42.0852934214155, -21.3404329964603));
            points.Add(new Vector2(42.4033728716175, -19.9893393258885));
            points.Add(new Vector2(42.3237557154947, -20.6494092058026));
            points.Add(new Vector2(41.5441566802936, -16.0607546940506));
            points.Add(new Vector2(41.72412427307, -21.9234675195968));
            points.Add(new Vector2(32.4641642484322, -21.3002599027387));
            points.Add(new Vector2(39.9352522341487, -23.9462892677804));
            points.Add(new Vector2(34.5713472318392, -14.8254621475697));
            points.Add(new Vector2(33.5827782229349, -15.7224764684538));
            points.Add(new Vector2(42.450226765053, -18.7065868588601));
            points.Add(new Vector2(33.672361364421, -15.7455240231297));
            points.Add(new Vector2(32.5284392877072, -17.6657374938979));
            points.Add(new Vector2(37.892113781867, -14.1517933525374));
            points.Add(new Vector2(41.4280820184052, -16.0430692825526));
            points.Add(new Vector2(33.1277181054769, -16.3531826056688));
            points.Add(new Vector2(33.0227974472626, -16.4427378745976));
            points.Add(new Vector2(33.966285386487, -15.3301410433736));
            points.Add(new Vector2(36.4264956494693, -24.4953487675986));
            points.Add(new Vector2(40.5044242583337, -15.2793086831183));



            var pointSet = new PointCloud2(points);

            var voronoi = AutomationLibrary.Mathematics.Geometry.Voronoi.VoronoiDiagram.ComputeForPoints(points);
            voronoi = voronoi.Filter(0); // build map of nearest points

            var centersOfInfiniteCells = new HashSet<Vector2>();
            foreach (var edge in voronoi.Edges)
            {
                if (edge.IsPartlyInfinite)
                {
                    centersOfInfiniteCells.Add(edge.LeftData);
                    centersOfInfiniteCells.Add(edge.RightData);
                }
            }

            var pointSet2 = new PointCloud2(centersOfInfiniteCells);

            var mcc = pointSet2.ComputeMinimumCircumscribingCircle();
            var mic = ComputeMaximumInscribedCircle(pointSet, voronoi, mcc);
            var lsc = GeometricFits.FitCircle(points);

            using (var writer = System.IO.File.CreateText(@"C:\users\douglas\desktop\circlepoints.csv"))
            {
                writer.WriteLine("X,Y");
                foreach (var point in pointSet)
                {
                    writer.WriteLine("{0},{1}", point.X, point.Y);
                }
            }

            Console.WriteLine("n = {0}", points.Count);
            Console.WriteLine("MIC @ ({0}), r = {1}", mic.Center, mic.Radius);
            Console.WriteLine("LSC @ ({0}), r = {1}", lsc.Center, lsc.Radius);
            Console.WriteLine("MCC @ ({0}), r = {1}", mcc.Center, mcc.Radius);

            Console.WriteLine();

            Console.WriteLine("draw.circle({0}, {1}, {2}, border='{3}')", mic.Center.X, mic.Center.Y, mic.Radius, "red");
            Console.WriteLine("draw.circle({0}, {1}, {2}, border='{3}')", lsc.Center.X, lsc.Center.Y, lsc.Radius, "blue");
            Console.WriteLine("draw.circle({0}, {1}, {2}, border='{3}')", mcc.Center.X, mcc.Center.Y, mcc.Radius, "green");

            Console.ReadLine();
        }

        private static Circle2 ComputeMaximumInscribedCircle(PointCloud2 points, AutomationLibrary.Mathematics.Geometry.Voronoi.VoronoiDiagram voronoi, Circle2 mcc)
        {
            var candidateCenters = new List<Vector2>();
            candidateCenters.AddRange(voronoi.Vertices.Keys);
            // TODO: add intersections between voronoi edges and convex hull of points

            Circle2 incumbent = new Circle2(points.First(), 0);

            foreach (var candidate in candidateCenters)
            {
                foreach (var neighbor in voronoi.Vertices[candidate])
                {
                    var candidateRadius = Vector2.DistanceBetweenPoints(candidate, neighbor);

                    if (candidateRadius > incumbent.Radius && mcc.Contains(candidate))
                    {
                        incumbent = new Circle2(candidate, candidateRadius);
                    }
                }
            }

            return incumbent;
        }

        static void ProfileTest() 
        {
            var spec = PartSpecification.Parse(@"F:\ExampleGuideWireProfileSpec.xml");




            List<Vector2> points = new List<Vector2>();

            using (var reader = System.IO.File.OpenText(@"F:\wire-profile.csv"))
            {
                reader.ReadLine(); // skip header
                
                while (true)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) break;

                    var segments = line.Split(',');

                    double x, y;
                    if (!double.TryParse(segments[0], out x)) throw new ApplicationException();
                    if (!double.TryParse(segments[1], out y)) throw new ApplicationException();

                    points.Add(new Vector2(x, y));
                }
            }

            var simplified = AutomationLibrary.Mathematics.Curves.Polyline.Simplify(points, 0.0003);

            var fitLines = new List<Tuple<double,double,Line2>>();

            for (int i = 0; i < simplified.Count - 1; i++)
            {
                var start = simplified[i];
                var end = simplified[i + 1];

                //if ((end.X - start.X) < .2) break;

                var segPoints = SelectPointsBetween(points, start.X, end.X);
                var segRoughLine = GeometricFits.FitLine(segPoints);
                var segFineLine = segRoughLine;

                var n = segPoints.Count;

                if (n > 20)
                {
                    var skip = n / 20;
                    var residuals = from p in segPoints
                                    select Tuple.Create(segRoughLine.DistanceFromLine(p), p);
                    var residualsNotNearEnds = residuals.Skip(skip).Take(n - 2 * skip);
                    var nonOutliers = from r in residualsNotNearEnds
                                      orderby r.Item1 descending
                                      select r.Item2;
                    var nonOutlierPoints = nonOutliers.Skip(skip).ToArray();

                    segFineLine = GeometricFits.FitLine(nonOutlierPoints);
                }

                fitLines.Add(Tuple.Create(start.X, end.X, segFineLine));
            }

            fitLines.RemoveAt(12);
            fitLines.RemoveAt(7);

            using (var writer = System.IO.File.CreateText(@"F:\profile-fit-lines.csv"))
            {
                writer.WriteLine("X,Y");
                writer.WriteLine("{0},{1}", fitLines[0].Item1, fitLines[0].Item3.Intercept + fitLines[0].Item3.Slope * fitLines[0].Item1);

                for (int i = 0; i < fitLines.Count - 1; i++)
                {
                    var cross = Line2.Intersection(fitLines[i].Item3, fitLines[i + 1].Item3);
                    if (cross.HasValue)
                    {
                        writer.WriteLine("{0},{1}", cross.Value.X, cross.Value.Y);
                    }
                    else throw new Exception();
                }

                var lastIdx = fitLines.Count - 1;
                writer.WriteLine("{0},{1}", fitLines[lastIdx].Item2, fitLines[lastIdx].Item3.Intercept + fitLines[lastIdx].Item3.Slope * fitLines[lastIdx].Item2);
            }

            Console.WriteLine("Profile processed.");
            Console.WriteLine();

            Console.ReadLine();
        }

        static List<Vector2> SelectPointsBetween(IList<Vector2> points, double left, double right)
        {
            return (from p in points
                    where (p.X >= left) && (p.X <= right)
                    select p).ToList();
        }

        static void PlaneFittingTest()
        {
            var points = new List<Vector3>();

            var actualNormal = new Vector3(2, 7, -4).Normalize();
            var actualDistanceFromOrigin = 417.3;

            Console.WriteLine("Actual: {0}", actualNormal);
            Console.WriteLine("Actual: {0}", actualDistanceFromOrigin);
            Console.WriteLine();

            var rand = new Random();

            var planeCenterPoint = actualNormal * actualDistanceFromOrigin;
            var yAxis = Vector3.CrossProduct(Vector3.UnitX, actualNormal).Normalize();
            var xAxis = Vector3.CrossProduct(yAxis, actualNormal).Normalize();


            for (int i = 0; i < 50000; i++)
            {
                var xOnPlane = rand.NextDouble() * 50 - 25;
                var yOnPlane = rand.NextDouble() * 50 - 25;

                var noiseOffPlane = rand.NextDouble() * 10 - 5;

                var point = planeCenterPoint + (xOnPlane * xAxis) + (yOnPlane * yAxis) + (noiseOffPlane * actualNormal);
                points.Add(point);
            }
            

            var plane = GeometricFits.FitPlane(points);

            Console.WriteLine("Fit: {0}", plane.Normal);
            Console.WriteLine("Fit: {0}", plane.SignedDistanceFromPlane(Vector3.Zero));
        }
    }
}
