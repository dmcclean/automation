using System;
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

            points.Add(new Vector2(33.3481493025898, -22.6174521602905));
            points.Add(new Vector2(36.5069813748174, -24.3206513431177));
            points.Add(new Vector2(32.2260920071029, -20.3116177204866));
            points.Add(new Vector2(33.1732747312248, -22.2610413980622));
            points.Add(new Vector2(41.9319414125878, -16.567098538206));
            points.Add(new Vector2(36.0283564790211, -24.3543016248646));
            points.Add(new Vector2(40.7655040922855, -15.2468975313432));
            points.Add(new Vector2(34.4339158873685, -15.0278509061006));
            points.Add(new Vector2(42.6659834352447, -18.9024197854612));
            points.Add(new Vector2(32.529039723949, -21.4334779337923));
            points.Add(new Vector2(33.7314584154313, -22.8672278263156));
            points.Add(new Vector2(35.5929504195901, -24.1528402552028));
            points.Add(new Vector2(33.2761132657987, -15.9129518275197));
            points.Add(new Vector2(37.5277119781475, -14.2170426387526));
            points.Add(new Vector2(33.0524969961008, -22.0588498865972));
            points.Add(new Vector2(39.8865936008224, -23.7994902919426));
            points.Add(new Vector2(42.7545111047803, -19.293839714613));
            points.Add(new Vector2(42.3350182586689, -20.77022177746));
            points.Add(new Vector2(33.9660066910794, -23.0917234494188));
            points.Add(new Vector2(42.447986423588, -18.4071319886684));
            points.Add(new Vector2(42.4252868193215, -20.116512392702));
            points.Add(new Vector2(32.0860967414889, -19.436552206234));
            points.Add(new Vector2(38.0063788509255, -24.5475467842213));
            points.Add(new Vector2(40.0747498670115, -14.6883331487998));
            points.Add(new Vector2(41.3174823851688, -22.7095235673497));
            points.Add(new Vector2(38.5821513082304, -14.3851444659216));
            points.Add(new Vector2(35.3436607882092, -14.6027220326429));
            points.Add(new Vector2(33.070786020862, -16.5000356559169));
            points.Add(new Vector2(39.116073404524, -24.3322012917138));
            points.Add(new Vector2(41.6926249303254, -16.2804883540131));
            points.Add(new Vector2(40.0029883219049, -14.63022047003));
            points.Add(new Vector2(34.6697278897917, -15.0553394662647));
            points.Add(new Vector2(40.5301458942361, -23.5205530299641));
            points.Add(new Vector2(42.6376937148146, -20.0124146393748));
            points.Add(new Vector2(35.0978790371892, -23.913064691568));
            points.Add(new Vector2(41.3648435259589, -22.8687101690214));
            points.Add(new Vector2(33.0405938584394, -16.7097316711441));
            points.Add(new Vector2(39.0906974539612, -14.3824563766374));
            points.Add(new Vector2(40.6714614411959, -15.1866401146543));
            points.Add(new Vector2(39.7025232860989, -23.8569584637194));
            points.Add(new Vector2(42.320192641604, -21.1936219816718));
            points.Add(new Vector2(42.6235626317971, -20.4027255878799));
            points.Add(new Vector2(42.4587301278097, -20.5568696612986));
            points.Add(new Vector2(42.5427664133101, -19.8863265407248));
            points.Add(new Vector2(33.3875420671785, -16.0785237343763));
            points.Add(new Vector2(34.5515226500783, -15.0174086516359));
            points.Add(new Vector2(33.9672770755982, -15.3297492067445));
            points.Add(new Vector2(42.5541105779767, -19.7070566819003));
            points.Add(new Vector2(35.6668811763894, -14.4030132976845));
            points.Add(new Vector2(39.6858019254392, -23.8433269906788));
            points.Add(new Vector2(39.0749842965031, -24.289884491535));
            points.Add(new Vector2(35.6601881288708, -24.328970456387));
            points.Add(new Vector2(32.1912116758096, -18.7132415013368));
            points.Add(new Vector2(32.6859205928295, -21.5229429916452));
            points.Add(new Vector2(42.2983948815669, -20.8694196047176));
            points.Add(new Vector2(38.1814579242395, -14.1675680579621));
            points.Add(new Vector2(35.352714706565, -14.6621872421044));
            points.Add(new Vector2(37.2825028085505, -14.0180508440897));
            points.Add(new Vector2(34.7152053705972, -14.9540397557926));
            points.Add(new Vector2(42.3143623983031, -18.021841898384));
            points.Add(new Vector2(33.2980003085618, -16.2650451432712));
            points.Add(new Vector2(41.9216911902246, -22.0114058614255));
            points.Add(new Vector2(42.4745332361413, -20.7673967486925));
            points.Add(new Vector2(41.0097025832701, -23.0552126862237));
            points.Add(new Vector2(33.7831865595251, -15.4952879360086));
            points.Add(new Vector2(41.640583320336, -16.4782823134109));
            points.Add(new Vector2(32.4825597497408, -21.356766050492));
            points.Add(new Vector2(32.3194213797669, -20.8859518259781));
            points.Add(new Vector2(35.7221081753723, -24.2636866770008));
            points.Add(new Vector2(36.3593183239274, -24.4137872062944));
            points.Add(new Vector2(42.4017465852244, -21.0078811504706));
            points.Add(new Vector2(34.1494856583057, -23.3773620880685));
            points.Add(new Vector2(40.6185110667724, -15.2548094792438));
            points.Add(new Vector2(42.1146816632864, -17.1284768390896));
            points.Add(new Vector2(33.914999241868, -15.2410538220503));
            points.Add(new Vector2(42.6275320092035, -19.9234227607863));
            points.Add(new Vector2(42.5310940387278, -19.8521291737205));
            points.Add(new Vector2(35.655063126102, -14.4290930978982));
            points.Add(new Vector2(38.3708313100621, -14.2421663266249));
            points.Add(new Vector2(42.2681857923029, -17.7279272076896));
            points.Add(new Vector2(35.7761411411465, -24.3851045410059));
            points.Add(new Vector2(35.4194844719439, -24.0223245244731));
            points.Add(new Vector2(41.8709953296976, -16.4663736823783));
            points.Add(new Vector2(42.4946099991295, -18.2500100260227));
            points.Add(new Vector2(35.7349755719987, -14.3524119767096));
            points.Add(new Vector2(32.7553930455143, -16.903748839493));
            points.Add(new Vector2(34.7578365303393, -14.8003281743766));
            points.Add(new Vector2(32.3000984412209, -18.321086908721));
            points.Add(new Vector2(37.2639639125851, -13.9666342859352));
            points.Add(new Vector2(40.4949987628752, -15.1535320552911));
            points.Add(new Vector2(34.1605153826175, -15.307595581259));
            points.Add(new Vector2(32.3208915544146, -20.7792351879101));
            points.Add(new Vector2(42.2105318438148, -17.519222143454));
            points.Add(new Vector2(42.6863967622496, -19.0321667857758));
            points.Add(new Vector2(32.1761895141722, -18.94586185027));
            points.Add(new Vector2(42.4553309475227, -19.5046707109366));
            points.Add(new Vector2(32.6372965909628, -17.0059074009896));
            points.Add(new Vector2(33.2159429724557, -16.1178205594792));
            points.Add(new Vector2(32.2808405687922, -18.6695951481212));
            points.Add(new Vector2(34.0907984644647, -23.2023463549663));
            points.Add(new Vector2(40.9317167263433, -15.637218524654));
            points.Add(new Vector2(35.9278492660868, -14.1905478418574));
            points.Add(new Vector2(34.1345932360462, -23.3148583997326));
            points.Add(new Vector2(32.5378406600464, -20.8526288015273));
            points.Add(new Vector2(33.4306256559234, -16.0601981590979));
            points.Add(new Vector2(41.8173878647379, -22.0289608076476));
            points.Add(new Vector2(40.2359192563989, -23.7613976055168));
            points.Add(new Vector2(38.4567250794464, -24.2588209919093));
            points.Add(new Vector2(32.9424611431087, -16.5130750294807));
            points.Add(new Vector2(38.6794749265436, -24.2272618063245));
            points.Add(new Vector2(39.8287171198203, -14.5791279733271));
            points.Add(new Vector2(41.8863098570676, -22.2174713073659));
            points.Add(new Vector2(42.6566062704552, -20.1303195764935));
            points.Add(new Vector2(34.4213029928646, -15.1150336394987));
            points.Add(new Vector2(39.7847478257977, -14.5803719421558));
            points.Add(new Vector2(32.2025661542709, -19.1255166460508));
            points.Add(new Vector2(34.9253882039167, -14.5593722472091));
            points.Add(new Vector2(39.3207020488368, -14.3065298871842));
            points.Add(new Vector2(42.7000019728131, -18.6308263062538));
            points.Add(new Vector2(34.8751393438142, -23.825507912158));
            points.Add(new Vector2(32.3570563544915, -17.8092135355708));
            points.Add(new Vector2(41.1226524426575, -22.7119441536294));
            points.Add(new Vector2(37.204164117345, -24.5888386570133));
            points.Add(new Vector2(32.4963091860285, -20.8101212890853));
            points.Add(new Vector2(32.066038414305, -19.4892783238419));
            points.Add(new Vector2(34.5932637764451, -23.7930449596842));
            points.Add(new Vector2(37.3390366531021, -24.42603642365));
            points.Add(new Vector2(38.1008941579185, -24.4607633826992));
            points.Add(new Vector2(33.3010977858886, -22.4149003047575));
            points.Add(new Vector2(38.9152274986621, -24.2135950247718));
            points.Add(new Vector2(42.0236692278453, -21.6552357208689));
            points.Add(new Vector2(34.8522772830711, -23.7514316879383));
            points.Add(new Vector2(36.1783055577088, -14.1336233237419));
            points.Add(new Vector2(32.676826853567, -21.1531949991896));
            points.Add(new Vector2(32.9598910189645, -16.6606276354776));
            points.Add(new Vector2(42.5951292544652, -18.2541929259858));
            points.Add(new Vector2(32.7823777724432, -21.7852543121932));
            points.Add(new Vector2(39.3902826658902, -24.1259515033736));
            points.Add(new Vector2(42.6532364533555, -19.8887195650457));
            points.Add(new Vector2(32.3934395115417, -20.6452618251431));
            points.Add(new Vector2(34.97731496294, -24.0651307330289));
            points.Add(new Vector2(37.6835012388612, -14.171614533552));
            points.Add(new Vector2(42.4238291261655, -18.5764316225865));
            points.Add(new Vector2(38.7019965170606, -24.2786585265542));
            points.Add(new Vector2(36.9413627024593, -14.2027910416784));
            points.Add(new Vector2(39.0246824850934, -14.3661570869888));
            points.Add(new Vector2(39.3520041036635, -14.4161677599912));
            points.Add(new Vector2(42.3186723364804, -20.9995489053936));
            points.Add(new Vector2(32.4139430295758, -18.2154278129782));
            points.Add(new Vector2(39.6192912587316, -14.5890309130356));
            points.Add(new Vector2(34.2059660683262, -23.5825499842687));
            points.Add(new Vector2(41.824673521358, -22.1737685801993));
            points.Add(new Vector2(41.6066330876594, -22.1049816632639));
            points.Add(new Vector2(33.130210710074, -16.2374062580038));
            points.Add(new Vector2(34.1911320634567, -15.1881997399417));
            points.Add(new Vector2(32.2294783934915, -20.1248110077705));
            points.Add(new Vector2(36.5513977006395, -24.4295853545586));
            points.Add(new Vector2(34.7728643983536, -23.8983607054774));
            points.Add(new Vector2(32.6457818552759, -21.6441252059506));
            points.Add(new Vector2(33.7326432671782, -22.98142347185));
            points.Add(new Vector2(39.1695152772318, -24.1834464746706));
            points.Add(new Vector2(42.5224071103749, -18.0637774056398));
            points.Add(new Vector2(38.4862066682395, -24.2937306055679));
            points.Add(new Vector2(35.7491308837396, -24.2588525744474));
            points.Add(new Vector2(32.9740218189457, -16.6159141988631));
            points.Add(new Vector2(32.4063278947108, -20.5620341073686));
            points.Add(new Vector2(38.5563491484538, -14.3635477092759));
            points.Add(new Vector2(32.4081614460214, -21.0580690638648));
            points.Add(new Vector2(41.8675543558586, -21.6747851602399));
            points.Add(new Vector2(32.4029651812628, -17.9315521895046));
            points.Add(new Vector2(34.3604358567095, -23.6035362124513));
            points.Add(new Vector2(34.2834547619509, -15.1062181152355));
            points.Add(new Vector2(40.8189190161148, -15.4140192187334));
            points.Add(new Vector2(39.5017195242916, -14.6810656763336));
            points.Add(new Vector2(41.8174915708811, -16.5885310421753));
            points.Add(new Vector2(36.8338143103663, -13.9804157812797));
            points.Add(new Vector2(38.2054396150093, -14.2819718478123));
            points.Add(new Vector2(35.2398285045402, -23.9593852107851));
            points.Add(new Vector2(42.5193254454394, -20.7199070356405));
            points.Add(new Vector2(38.1411020021853, -24.5309901999683));
            points.Add(new Vector2(35.8014620728149, -14.3368238245886));
            points.Add(new Vector2(33.1163446007085, -22.3033735631572));
            points.Add(new Vector2(42.439143405355, -18.4713430262879));
            points.Add(new Vector2(32.3908303250217, -20.9260751997828));
            points.Add(new Vector2(33.1652548414119, -22.2256889434531));
            points.Add(new Vector2(42.3771785093545, -20.1646319920668));
            points.Add(new Vector2(41.5515898995795, -22.6005943303741));
            points.Add(new Vector2(42.6855872320749, -18.4661701710697));
            points.Add(new Vector2(35.9152153187043, -14.1835957293532));
            points.Add(new Vector2(32.9600720695646, -22.2715730196857));
            points.Add(new Vector2(41.3071701937273, -15.7010149912963));
            points.Add(new Vector2(32.2243045905423, -20.1745582305139));
            points.Add(new Vector2(42.3517756982419, -20.5222344841545));
            points.Add(new Vector2(42.4121796284653, -20.3711590046388));
            points.Add(new Vector2(42.661260444778, -20.0474986748878));
            points.Add(new Vector2(42.0176953692874, -16.9989079486777));
            points.Add(new Vector2(33.5063985533859, -22.659923927335));
            points.Add(new Vector2(40.7521368974985, -15.3867198285347));
            points.Add(new Vector2(32.3675163615217, -19.7995400701322));
            points.Add(new Vector2(32.9107608850165, -21.9094733722203));
            points.Add(new Vector2(41.3779710134835, -16.0936658244504));
            points.Add(new Vector2(34.2220765138077, -23.3882423126781));
            points.Add(new Vector2(33.8633956517836, -15.5060477951951));
            points.Add(new Vector2(33.3527699644219, -22.3390658292239));
            points.Add(new Vector2(39.3592620784113, -14.5337874601998));
            points.Add(new Vector2(33.0527450002831, -21.865004165896));
            points.Add(new Vector2(42.2766942446323, -20.7091746538253));
            points.Add(new Vector2(36.7024579025343, -14.0585470676553));
            points.Add(new Vector2(40.1476605649611, -14.7317207445451));
            points.Add(new Vector2(32.0824360134831, -19.7240273619457));
            points.Add(new Vector2(37.4671741742543, -14.0720765659676));
            points.Add(new Vector2(40.4540374926769, -14.9829779392606));
            points.Add(new Vector2(39.0396315432665, -14.2753965462484));
            points.Add(new Vector2(39.5449164671537, -23.914977218002));
            points.Add(new Vector2(42.7020684899937, -19.5462361849762));
            points.Add(new Vector2(42.5160246465301, -19.650786834666));
            points.Add(new Vector2(34.5323530611934, -14.8698252991096));
            points.Add(new Vector2(42.0269459312486, -16.8039877506327));
            points.Add(new Vector2(40.0302255831205, -23.8651599650167));
            points.Add(new Vector2(40.8077306234285, -23.0411023679443));
            points.Add(new Vector2(42.3626347037502, -20.6703002553676));
            points.Add(new Vector2(42.6273937510297, -18.6012613550812));
            points.Add(new Vector2(42.2169351386319, -17.1548478506759));
            points.Add(new Vector2(42.5028268308931, -19.3254670961192));
            points.Add(new Vector2(39.1472250563305, -24.1921028415733));
            points.Add(new Vector2(41.0329151848158, -15.4251543338543));
            points.Add(new Vector2(42.6856265862092, -18.5687033014904));
            points.Add(new Vector2(40.950798868184, -15.3958640880148));
            points.Add(new Vector2(36.4987488417784, -24.4296656281205));
            points.Add(new Vector2(40.8066412968709, -23.1437393026722));
            points.Add(new Vector2(42.3049742930014, -17.2340519421647));
            points.Add(new Vector2(32.9186339940807, -22.0648753321099));
            points.Add(new Vector2(42.2213801022922, -21.0105587676165));
            points.Add(new Vector2(32.2263159960848, -19.6467261472604));
            points.Add(new Vector2(38.3729278531915, -14.0966291691359));
            points.Add(new Vector2(42.0576799206213, -21.9350350719327));
            points.Add(new Vector2(42.4964379853491, -17.6833065359277));
            points.Add(new Vector2(34.2437496420483, -23.4203606359522));
            points.Add(new Vector2(32.4852279340423, -17.3965676943916));
            points.Add(new Vector2(39.2439421511581, -14.4197109634668));
            points.Add(new Vector2(32.3573393699044, -19.6398878891901));
            points.Add(new Vector2(37.5431371660449, -13.9694871171122));
            points.Add(new Vector2(32.1446115834759, -18.3173386662888));
            points.Add(new Vector2(39.7622272149782, -14.6515459292082));
            points.Add(new Vector2(32.5412868034787, -17.6149234740543));
            points.Add(new Vector2(34.5686050865983, -23.5817412367977));
            points.Add(new Vector2(32.5093344746529, -17.7495995270874));
            points.Add(new Vector2(41.4898260657311, -22.2681088190208));
            points.Add(new Vector2(37.751447695002, -14.101001222672));
            points.Add(new Vector2(38.4957433737691, -14.0747153103637));
            points.Add(new Vector2(32.3643997039577, -18.9741380355302));
            points.Add(new Vector2(32.0620986375896, -19.0439073959294));
            points.Add(new Vector2(39.9410171182682, -14.9266405965767));
            points.Add(new Vector2(32.6366062988875, -17.5397724582744));
            points.Add(new Vector2(41.1424037623884, -15.5833458823118));
            points.Add(new Vector2(34.8522830358718, -14.7601027779167));
            points.Add(new Vector2(37.2048561542557, -24.3855414992983));
            points.Add(new Vector2(41.5880623488416, -22.5017529326228));
            points.Add(new Vector2(33.5667195556628, -22.7435898074444));
            points.Add(new Vector2(39.1886861906378, -24.174976539776));
            points.Add(new Vector2(39.7394998759319, -24.0334062971886));
            points.Add(new Vector2(33.2642900878009, -16.0978901236963));
            points.Add(new Vector2(39.8826410918212, -23.8811589200913));
            points.Add(new Vector2(42.4489714886548, -20.1173799162191));
            points.Add(new Vector2(32.2651493842887, -20.2927411439019));
            points.Add(new Vector2(32.0687037675944, -19.1517195894106));
            points.Add(new Vector2(32.3762248069031, -18.7106580383307));
            points.Add(new Vector2(40.5312799267867, -23.6147476243672));
            points.Add(new Vector2(40.046848878704, -23.7683448614715));
            points.Add(new Vector2(33.0214273864685, -21.844022795008));
            points.Add(new Vector2(42.3834023322153, -18.4146130363325));
            points.Add(new Vector2(35.4496557802072, -14.3984793950864));
            points.Add(new Vector2(37.0734077196354, -24.5661114671138));
            points.Add(new Vector2(33.2488247487453, -22.3345097619567));
            points.Add(new Vector2(40.1559659062661, -14.8369453631368));
            points.Add(new Vector2(33.967367449362, -23.1210322305769));
            points.Add(new Vector2(41.7092803548962, -16.2228424018928));
            points.Add(new Vector2(33.3300509975471, -15.8647479953446));
            points.Add(new Vector2(42.2603653857363, -21.0738655065535));
            points.Add(new Vector2(34.6136959272253, -23.5559942965629));
            points.Add(new Vector2(42.3816392833002, -17.4873140128337));
            points.Add(new Vector2(34.8215592896966, -23.6565901109214));
            points.Add(new Vector2(32.6169321264936, -21.1155762865037));
            points.Add(new Vector2(32.6732628867556, -21.3962859757864));
            points.Add(new Vector2(41.3361253683287, -15.8941343871729));
            points.Add(new Vector2(33.6672103172507, -22.7916399553151));
            points.Add(new Vector2(35.4053725075847, -23.9579989984423));
            points.Add(new Vector2(35.8276653109345, -14.3101985540648));
            points.Add(new Vector2(39.6995731395995, -23.8144991793259));
            points.Add(new Vector2(42.6713564466136, -19.9450575254438));
            points.Add(new Vector2(41.7084953955131, -21.9423240038987));
            points.Add(new Vector2(36.0600486434597, -14.2240072402298));
            points.Add(new Vector2(33.5427891840361, -22.8175639229987));
            points.Add(new Vector2(36.4619393926538, -24.3760596998389));


            var pointSet = new PointCloud2(points);

            var mcc = pointSet.ComputeMinimumCircumscribingCircle();
            var lsc = GeometricFits.FitCircle(points);

            Console.ReadLine();
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
