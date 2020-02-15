using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    private static readonly string[] names =
    {
"Aaron","Abba","Abbatissa","Abbo","Abednego","Abel","Abigail","Abone","Abraham","Absalom","Abundance","Acco","Accorsa","Aceline","Acfrid","Acfrida","Achard","Achere","Acherea","Achilde","Achilles","Aclebalda","Acleberta","Acledulf","Aclefrid","Aclehar","Aclehard","Aclehilde","Acleman","Aclemund","Acletrude","Aclewalda","Aclinde","Aclulf","Actard","Actwin","Ada","Adalald","Adalbald","Adalbod","Adaldag","Adaleus","Adalfrid","Adalfrida","Adalgard","Adalgarde","Adalgaria","Adalgaud","Adalger","Adalgilde","Adalginde","Adalgis","Adalgisa","Adalgisdis","Adalgod","Adalgrim","Adalgude","Adalgunde","Adalhar","Adalhelm","Adalhilde","Adalhoh","Adalinde","Adalmar","Adalmod","Adalmund","Adalrad","Adalrada","Adalram","Adalsad","Adalsada","Adalsinde","Adaltrude","Adalwald","Adalward","Adaly","Adam","Adberg","Adegar","Adela","Adeline","Adelo","Ademar","Aderich","Adiel","Adjutor","Admirable","Admiranda","Ado","Adolf","Adrebald","Adrebert","Adrehar","Adrehilde","Adremar","Adrewic","Adrian","Adriana","Adrulf","Advocat","Africana","Aga","Agapetus","Agatha","Agbert","Agenilde","Agerbert","Agilmode","Agina","Aginbert","Aginbod","Agino","Aginteus","Agintrude","Aginulf","Aglinde","Agmund","Agnes","Ago","Agulf","Ahasuerus","Aicard","Aicfrida","Aichild","Aico","Aicusa","Ailbern","Ailbert","Ailgrim","Ailhard","Ailith","Ainard","Airard","Aitard","Aitilde","Aitla","Aitland","Aitulf","Aizivella","Alafrid","Alamand","Alamanda","Alan","Alana","Alanteus","Alard","Alaric","Alba","Alban","Albana","Alberich","Albero","Albert","Alberta","Albilde","Albin","Albo","Albrand","Albulf","Alburg","Alcfrida","Alcteus","Alctrude","Alcwin","Alda","Aldebrand","Aldegarde","Aldeger","Aldeman","Aldemar","Alden","Aldetrude","Aldfred","Aldhelm","Aldith","Aldiva","Aldo","Aldrada","Aldram","Aldrich","Aldulf","Aldwin","Alethea","Alexander","Alexandra","Alexis","Alf","Alfdag","Alfgar","Alfheah","Alfhelm","Alfhild","Alfred","Alfstan","Alfsy","Alfwald","Alfwin","Alibert","Alice","Alinbert","Alinhilde","Aliva","Alker","Alleaume","Allegra","Allegrezza","Allegro","Allo","Alphard","Alphonse","Alrich","Altabella","Altadonna","Altafrons","Altberga","Altbert","Altberta","Alteria","Altilde","Altrude","Alvaro","Alvice","Alviva","Alwi","Alwin","Ama","Amabel","Amadeus","Amadilde","Amadore","Amalberg","Amalbert","Amalfrid","Amalfrida","Amalgarde","Amalgaud","Amalger","Amalgilde","Amalher","Amalhilde","Amaltrude","Amalwald","Amalwin","Amanda","Amant","Amat","Amata","Amblard","Ambrich","Ambrose","Ambrosia","Amelia","Ameria","Ami","Amice","Amicus","Amos","Amy","Ananias","Anastasia","Anastasius","Ancel","Andrea","Andrew","Andry","Angel","Angela","Angelica","Angharad","Anglicus","Anima","Anne","Ansbald","Ansbert","Ansberta","Ansbrand","Anschetil","Ansegaud","Ansegilde","Anselm","Anselma","Anserich","Ansfrid","Ansgar","Ansgarde","Ansgot","Ansoin","Ansois","Ansold","Answard","Antelm","Antenor","Anthony","Antonia","Anzo","Aodh","Apollonia","Apollonius","April","Aquila","Aquilina","Archangel","Archibald","Arcwin","Aregia","Argenta","Aristotle","Arnbert","Arnberta","Arnger","Arngilde","Arngisl","Arnold","Arnold-William","Arnolde","Arntrude","Arnulf","Artald","Artcar","Arthfael","Arthuiu","Arthur","Arvid","Asa","Asculf","Ascwin","Ashwy","Aster","Athanasius","Atilia","Auchier","Audrad","Audrada","Audrey","Aufrey","August","Aurelia","Aurelian","Aurelius","Aurisma","Austin","Austine","Autbert","Autlaic","Autlinde","Ava","Averroes","Avice","Avo","Aylmer","Aylward",
"Baius","Balda","Baldbert","Baldegilde","Baldemar","Baldo","Baldowald","Baldrich","Baldulf","Baldwar","Baldwin","Baldwina","Balsinde","Baltad","Balthasar","Balthasara","Bando","Baptist","Barbara","Barnabas","Baron","Bartholomea","Bartholomew","Baruch","Basil","Basile","Bastard","Bathsheba","Baudran","Beata","Beatrice","Beauoncle","Beauvis","Belhonor","Belisarius","Bellacara","Belladonna","Bellaflor","Bellavita","Belleflos","Bellissima","Bellissimo","Benceline","Benedict","Benedicta","Benegar","Benjamin","Benvenuta","Benvenuto","Bera","Berard","Berarde","Berengar","Berengaria","Berfrid","Bergama","Bergamo","Berich","Berlinde","Berlwin","Bermund","Bernard","Bernard-William","Bernarde","Bernegilde","Bernewif","Bernhaus","Bernhilde","Bernier","Bernwald","Bernwin","Berohilde","Bert","Bertegar","Bertegilde","Bertfrid","Bertgaud","Bertha","Berthard","Berthild","Berthold","Bertier","Bertiere","Bertingaud","Bertisma","Bertleis","Bertrad","Bertrada","Bertram","Bertrande","Bertsinde","Bertulf","Bertwald","Bertwin","Berwald","Betta","Betto","Birgir","Bjorn","Blaise","Blanch","Blanche","Blanchefleur","Blasia","Blathilde","Bleddyn","Blitgilde","Blithe","Blither","Blithewine","Bogus","Boguslav","Bohemund","Bohun","Boleslav","Boleslava","Bona","Bonabella","Bonadeus","Bonafemina","Bonagiunta","Bonamice","Bonamicus","Bonanata","Bonaparte","Bonaventura","Bonaventure","Bonavera","Bonfides","Bonfilius","Boniface","Bonifacia","Bonissima","Bonitas","Bonjohn","Bono","Bonomo","Bonsimon","Bontalenta","Bontempo","Bonvalet","Boso","Bran","Brand","Branislav","Brendan","Breysia","Brian","Brice","Bridget","Brighthelm","Brightmer","Brightnod","Brihtrich","Brihtstan","Brithael","Brun","Bruna","Brunger","Brunhard","Brunissende","Brunwine","Brutus","Budislav","Budislava","Burchard",
"Cadell","Cadhoiarn","Cadwallader","Cadwallon","Cadwethen","Cadwgan","Cadwobri","Cadwored","Caesar","Caleb","Callistus","Calomaria","Calvin","Calvo","Camilla","Camille","Candid","Candida","Capuana","Cara","Carabella","Caradonna","Carissima","Carla","Carlfrid","Caro","Casimir","Casper","Caspera","Cassandra","Cassia","Cassian","Castellan","Castellana","Cecil","Cecilia","Celeste","Celestina","Celestine","Celestus","Cephas","Chapman","Charity","Charles","Cherin","Cherubina","Chloe","Christian","Christiana","Christin","Christina","Christofana","Christopher","Christophera","Christred","Christwin","Christwina","Clara","Clarabella","Clare","Clarembaut","Clarice","Claude","Claudia","Clemence","Clement","Cleopas","Columb","Columba","Comfort","Conbert","Concessus","Confortata","Conmarch","Conrad","Conrade","Consola","Consolat","Constance","Constant","Constantina","Constantine","Contaminat","Content","Conwal","Cora","Cormac","Cornelia","Cornelius","Cosmo","Countess","Craft","Crescent","Crescentia","Crispin","Crispina","Crispus","Cumdelu","Cunigunde","Cunihilda","Cunimund","Cuno","Cuthbert","Cuthberte","Cyneburg","Cynthius","Cyprian","Cyprianne","Cyril","Cyrila",
"Dada","Dadbert","Dado","Dagbert","Dagmar","Dago","Daguin","Dalbert","Dalibor","Dalmatia","Dalmatius","Damian","Damiana","Dan","Daniel","Darwin","David","Dea","Debonair","Deborah","Delicate","Demetrius","Denise","Dennis","Deocar","Deodata","Desiderius","Deurhoiarn","Dewnes","Diamond","Diana","Diego","Digory","Dimenche","Divitia","Dobeslav","Doctrama","Doda","Dodeus","Dodo","Dominic","Dominica","Donadei","Donagnesia","Donald","Donata","Donato","Donna","Dorcas","Dorothy","Douglass","Dragan","Dragon","Dragoslav","Drew","Driwethen","Dructbald","Dructbert","Druda","Drudmund","Drudo","Dudo","Dulce","Dulcedram","Dulcibelle","Duncan","Dunstan","Dunwine","Durand","Dusca",
"Ebbo","Ebrehar","Ebrulf","Ecco","Eckbert","Eckfrid","Eckhard","Eckrich","Eda","Edgar","Edith","Ediva","Edmer","Edmund","Edrich","Edward","Edwin","Edwina","Edwy","Ehud","Eilika","Einarr","Eleanor","Eleazar","Electa","Electelm","Electo","Electulf","Elegia","Elia","Elias","Elisanna","Elisaria","Elisiard","Elizabeth","Ella","Ellen","Eloise","Emerald","Emery","Emil","Emily","Emma","Emmanuel","Emmeline","Emmo","Engel","Engelbert","Engelfrid","Engelger","Engelhaid","Engelhard","Engelher","Engelman","Engelmar","Engelrich","Engelschalk","Engelsent","English","Enoch","Ephraim","Erasmus","Erchamberta","Erchamfred","Erchamger","Erchamhar","Erchamilde","Erchamold","Erchamrad","Erchamrich","Erchamtrude","Erchamwald","Erhard","Erik","Erlebald","Erlinde","Erlulf","Erma","Ermenbald","Ermenbalda","Ermenbert","Ermenberta","Ermenburg","Ermenfred","Ermengar","Ermengard","Ermengaud","Ermengaude","Ermengilde","Ermengod","Ermenhar","Ermenhard","Ermenhilde","Ermenold","Ermenrad","Ermenrich","Ermentar","Ermentaria","Ermentilde","Ermentrude","Ermenulf","Ermesinde","Ernest","Esperance","Esperanza","Esther","Eucharius","Eude","Eudemia","Eugene","Eugenia","Eulalia","Euphemia","Euphrosyne","Eupraxia","Eusebia","Eusebius","Eustace","Eustacia","Eutropius","Evangelist","Eve","Everard","Everbald","Everbern","Everbert","Evergrim","Everhelm","Everich","Evermar","Evermod","Everold","Everwin","Expert","Eyvind","Ezekiel",
"Faber","Fabian","Fabiana","Fabio","Fabrice","Fabrissa","Facetus","Faith","Falatrude","Falco","Falcona","Farbert","Farberta","Farolf","Father","Faust","Fausta","Felicia","Felician","Feliciana","Felicio","Felicitas","Felix","Femina","Fenicia","Ferald","Ferdinand","Fergal","Fergus","Ferrer","Filia","Finlay","Finn","Finnbogi","Finnian","Fionnghuala","Firmin","Flamen","Flaminio","Flora","Florabel","Florence","Florent","Floretia","Florian","Floridas","Focard","Formosus","Fortunate","Foucauld","Foy","Fraisende","Frambalda","Frambert","Framberta","Framenger","Framengilde","Framrich","Framtrude","Framwin","Frances","Francis","Frank","Franka","Fredebert","Fredegar","Fredegis","Fredemund","Frederica","Frederick","Frederius","Fredhelm","Fremy","Fridbert","Frideswide","Fridewald","Fritheburg","Frodeberga","Frodegard","Frodo","Frodohard","Frost","Frotbald","Frotbalda","Frotberga","Frotbert","Frotfrid","Frotgar","Frothilde","Frotlinde","Frotmund","Fruga","Fulbert","Fulcher","Fulk","Fulka","Fusca","Fuscian",
"Gabriel","Gabrielle","Gailhard","Gailhelm","Gainard","Gaius","Galea","Galicia","Galicius","Galiena","Gammo","Gandulf","Gangwolf","Garcia","Garet","Gaubert","Gaucelm","Gaucia","Gaudimia","Gaudiosus","Gautbert","Gautlinde","Gautmar","Gautrude","Gauzo","Gavin","Gebhard","Gebwin","Geldhart","Geldulf","Geldwin","Gemma","Genevieve","Gentile","Gentle","Geoffrey","George","Georgia","Gerald","Geralde","Gerard","Gerarde","Gerbald","Gerbalda","Gerberg","Gerbern","Gerbert","Gerberta","Gerbod","Gerbrand","Gerfrid","Gerhaus","Gerhelm","Gerhelma","Gerhilde","Gerhoh","Gerich","Gering","Gerlach","Gerlinde","Germain","Germaine","Germar","Germund","Gero","Gerosmus","Gersinde","Gertrude","Gerulf","Gervais","Gervaise","Gerward","Gerwig","Gerwin","Giambono","Gibeon","Gideon","Gilbald","Gilbert","Gilchrist","Giles","Gilia","Gilmar","Gilmor","Gilo","Giolla","Easpuig","Giolla","Íosa","Giolla","Mhíchíl","Gisa","Gisbern","Gisbert","Gisel","Gisela","Giselfrid","Giselhar","Giselhard","Giselmund","Giseltrude","Giselwin","Gisfrid","Gisland","Gislara","Gislilde","Gismunda","Giso","Gladwin","Goda","Godard","Godbald","Godbalda","Godbert","Godehild","Godelinde","Godfrey","Godiva","Godlanda","Godmar","Godo","Godric","Godsven","Godulf","Godwi","Godwin","Godwold","Golda","Goldhawk","Goldiva","Goldstone","Goldwine","Gontard","Gontarde","Gonzalo","Gospatric","Goswin","Gottschalk","Grace","Grassa","Grasso","Gratiadei","Gratiana","Gratiosa","Gratioso","Gratius","Gregoria","Gregory","Grima","Grimald","Grimbald","Grimbert","Grimhard","Grimher","Grimhilde","Grimulf","Grimwin","Griselda","Grossa","Grosso","Guardia","Gudlogh","Gudmund","Guerro","Guethencar","Guiart","Guilitsa","Guimar","Guimart","Guinevere","Guither","Gumbaud","Gumbert","Gumbrand","Gundesinde","Gundhold","Gundoilde","Gundred","Gundulf","Gundwin","Gunfred","Gunhild","Gunnora","Gunsa","Gunso","Guntbert","Gunther","Guntram","Gunwald","Gurhoiarn","Gustav","Guy","Guy-Geoffrey","Guya","Gwen","Gwenllian","Gwyn","Gwynhoiarn",
"Habakkuk","Haburg","Hachar","Hadburg","Hadda","Hadebert","Hadelinde","Hademan","Hademar","Hadena","Hadolf","Hadward","Haelcar","Haelhoiarn","Haelnou","Haeloc","Haelwaloe","Hagan","Haimbert","Haimengarde","Haimo","Hainard","Hainbert","Hainfroy","Hairich","Hakon","Haldor","Halfdan","Hannibal","Harda","Hardulf","Harger","Harold","Harriet","Hartger","Hartgilde","Hartman","Hartmar","Hartmund","Hartmut","Hartois","Hartrad","Hartrich","Hartwig","Hartwin","Harwich","Haward","Hawise","Hawk","Hector","Hedwig","Heidenrich","Heidentrude","Heilwig","Helga","Helgi","Helmbert","Helmburg","Helmdag","Helmhard","Helmold","Helmrich","Helmwald","Helmward","Hemard","Henarda","Henry","Herard","Herbald","Herbern","Herbert","Herberta","Herbrand","Hercules","Herilde","Herluin","Herman","Hermana","Hermanmar","Hermar","Hermes","Herois","Herrich","Hersent","Herulf","Herward","Herwin","Herzog","Hesperia","Hessa","Hesso","Hezekiah","Hezelo","Hilaria","Hilary","Hildebald","Hildebert","Hildeberta","Hildebod","Hildebrand","Hildeburg","Hildefrid","Hildegard","Hildegaud","Hildeger","Hildegilde","Hildegod","Hildegude","Hildegund","Hildelinde","Hildeman","Hildemar","Hildenibia","Hilderad","Hilderada","Hilderich","Hildesinde","Hildetrude","Hildewalde","Hildeward","Hildewin","Hildois","Hildrad","Hildrada","Hildulf","Hillinus","Hilpwin","Hippola","Hippolyta","Hippolytus","Hoger","Hohold","Holger","Holm","Holmbjorn","Holmsten","Homobon","Homodeus","Honest","Honesta","Honor","Honora","Honorat","Honorata","Honoria","Honorius","Horabona","Hrothgard","Hrotho","Hubald","Hubert","Hudrich","Hugh","Hugier","Hugran","Hulda","Huldegarde","Huldward","Huldwin","Humbaud","Humbelina","Humberga","Humbert","Humiliosus","Humphrey","Huno","Huward","Hyacinth","Hyacinthe","Hyssop","Hywel",
"Iarncum","Ida","Idalia","Idelinde","Ido","Idony","Ignatius","Illuminata","Imberg","Imbert","Imfrid","Imperia","Indigo","Infant","Inga","Ingalbald","Ingalrada","Ingalsinde","Ingaltrude","Ingarde","Ingbald","Ingbalda","Ingbert","Ingberta","Inge","Ingeborg","Ingibiorn","Ingigerd","Ingimar","Ingimund","Ingitrude","Ingram","Ingrid","Ingvald","Innocent","Innocentia","Isaac","Isabel","Isaiah","Isambert","Isarn","Isbrand","Ishmael","Isnard","Isoard","Isolde","Israel","Ithel","Ither","Iva","Ivar","Ivo",
"Jaca","Jacob","Jacoba","Jael","Jaelle","January","Jaromir","Jaroslav","Jaroslava","Jason","Jeremy","Jerome","Jeronima","Jesse","Joab","Joachim","Joachimie","Joan","Joan-Baptista","Joan-Stephanie","Job","Joceran","Jocosa","Joculus","Jodocus","John","John-Alphonse","John-Anthony","John-Baptist","John-Dominic","John-Francis","John-Jacob","John-Louis","John-Maria","John-Mark","John-Michael","John-Paul","John-Thomas","Johnbon","Johnson","Joly","Jonas","Jonathan","Jonilde","Jordan","Jordana","Josaphat","Joseph","Joshua","Josiah","Joy","Joyce","Judith","Julia","Julian","Juliana","Julius","Julius","Caesar","Justa","Justice","Justin","Justine","Justinian","Justiniana","Justrina","Justus",
"Kale","Katherin","Katherine","Ketilbern","Ketill","Kinborough","Knightwine","Knut",
"Laborans","Lambert","Lamberta","Lamond","Lance","Landbald","Landbod","Landelanda","Landetrude","Lando","Landrad","Landrada","Landrich","Landulf","Landward","Landwin","Lanfranc","Lanfrid","Langward","Lanselm","Lanswith","Lantberga","Lantelm","Lanter","Lanthar","Lantilde","Lanto","Laria","Latino","Lauger","Laura","Laurence","Laurencia","Lautard","Lautilde","Lavinia","Leah","Leander","Lefchild","Lefsy","Lefward","Lefwin","Leif","Lelio","Lella","Lena","Leo","Leona","Leonard","Leonarda","Leontius","Leopold","Lettice","Leudo","Libentius","Liberto","Lismod","Littera","Liutberga","Liutbert","Liutbrand","Liutfrid","Liutgarde","Liutgaud","Liutger","Liuthard","Liuthilde","Liuthold","Liutisma","Liutlinde","Liutmar","Liutmod","Liutrada","Liutrich","Liutulf","Liutward","Liutwin","Livia","Livy","Llywarch","Llywellyn","Lodulf","Lois","Lombard","Lothar","Lothgar","Louis","Louis-Arnold","Louisa","Loup","Loveday","Lovewell","Loyalty","Lucan","Luceria","Lucian","Luciana","Lucida","Lucius","Lucretia","Lucretius","Lucy","Luda","Luke","Luther","Luthera","Lydia",
"Maban","Macarius","Macbeth","Macduff","Machelm","Madalbert","Madalberta","Madalgaria","Madalger","Madalgude","Madalhilde","Madalinde","Madaltrude","Madalulf","Madalwin","Madog","Madwen","Maelgwn","Maenwallon","Maenwobri","Magdalene","Magna","Magner","Magnifica","Magnificus","Magnus","Maillard","Maitelm","Maiulf","Malachi","Malatesta","Malcolm","Malherbe","Malitia","Malparent","Manasses","Mancia","Mancinagross","Mancius","Mandisma","Manens","Manfred","Mangold","Manswith","Manwulf","Maol","Brigdhe","Maol","Domhnaigh","Maol","Dúin","Maol","Mhíchíl","Marcel","Marcella","Marcia","Marcswith","Marculf","Margaret","Maria","Mariantonia","Marianus","Marin","Marina","Marius","Mark","Mark-Antony","Marmaduke","Marozia","Marquart","Marquessa","Marsilius","Martha","Martin","Martin-Angel","Martina","Martio","Mary","Mary-Anne","Mary-Joan","Mary-Magdalene","Master","Materia","Mathilda","Mathurine","Matrona","Matthea","Matthew","Maubert","Mauger","Maura","Maurice","Mauricia","Maurin","Mauro","Maxim","Maxima","Maximiliana","Maynard","Meinbald","Meinberga","Meinbern","Meinbert","Meinfrid","Meinger","Meingod","Meingold","Meinhelm","Meinher","Meinhold","Meinilde","Meino","Meinsent","Meintrude","Meinulf","Meinward","Melanie","Melchior","Melior","Meliora","Membresia","Memorantia","Mercato","Mercury","Mercy","Meredith","Merme","Mermeta","Meshach","Mette","Meurwethen","Micah","Michael","Michael-Angel","Michaela","Michal","Mildred","Miles","Milia","Milicent","Milo","Miloslav","Minerva","Mira","Mirabel","Mirko","Miroslav","Monday","Montana","Mora","Morbida","Mordecai","Morgan","Moses","Muhammad","Muscata",
"Nadalberga","Nadalbert","Nadalfrid","Nadalger","Nadalinde","Nadalrad","Nadaltrude","Nanne","Nantelm","Nantelma","Nanthilde","Nantier","Naomi","Narcissa","Narcissus","Narduin","Natale","Natalie","Natalisma","Nathan","Nathaniel","Nemoy","Nestoria","Neville","Nicaise","Nicander","Nicephorus","Nicholas","Nicholas-Angel","Nicole","Nino","Nitard","Nivard","Nivo","Noah","Noel","Noelle","Noire","Norbert","Norman","Notger","Novel","Nymphidius",
"Octavian","Octavius","Odart","Odelbald","Odelberga","Odelbert","Odelbrand","Odelgarde","Odelgilde","Odelhard","Odelhaus","Odelhelm","Odelhilde","Odelschalk","Odierne","Odile","Odilo","Odilred","Odine","Odrich","Odulf","Ogo","Olaf","Olive","Oliver","Olivera","Olympia","Omnebon","Onofria","Onofrio","Opportune","Orienta","Orm","Orpah","Ortbert","Ortgar","Ortgis","Ortmar","Ortolf","Ortrich","Ortwin","Osanna","Osbern","Osbert","Osgyth","Osmund","Ostosia","Ostrobert","Osulf","Oswald","Oswi","Oswin","Otbert","Otes","Otfrid","Otleich","Otmar","Otnand","Otran","Otta","Ottabona","Ottilburg","Otto","Ottokar",
"Pacifica","Pagan","Pagana","Palm","Palma","Palmer","Palmeria","Paloma","Pandolf","Paradisa","Paris","Parisa","Parva","Pasca","Pascal","Pascale","Pask","Patience","Patricia","Patrick","Paul","Paula","Pax","Paxe","Pelagia","Pelagius","Penelope","Pentecoste","Percival","Peregrina","Peregrine","Perpetuo","Persis","Peter","Peter-Angel","Peter-Anthony","Peter-Paul","Petita","Petra","Petronilla","Philbert","Philip","Philipa","Phoebe","Piat","Placentius","Pleasant","Plectrude","Plena","Pomma","Pompei","Pompeia","Ponce","Poncia","Portia","Poubelle","Precious","Priam","Pridbor","Primavera","Priscilla","Procopius","Prospero","Protasia","Proxima","Prudence","Puglith","Pulchia","Pura",
"Quant","Quarto","Queniva","Quentin","Quentine","Quieton","Quintana","Quintius","Quirinus","Quiteria",
"Rabige","Rabot","Rabota","Rachel","Radax","Radegund","Radhilde","Radhold","Rado","Radoslav","Raimod","Ralph","Ralphe","Rambaud","Ramiro","Raphael","Raphaelle","Rascende","Ratbald","Ratberga","Ratbert","Ratberta","Ratelm","Ratfrid","Ratgis","Rathard","Rathelm","Ratibor","Ratimir","Ratrude","Raven","Ravenketil","Raymond","Raymond-Arnold","Raymond-Walter","Raymonda","Raymonda-Ameria","Rayner","Raynera","Rebecca","Recuperate","Regalis","Regina","Regula","Regulus","Reina","Reinbald","Reinbalda","Reinberga","Reinbern","Reinbert","Reinberta","Reinbod","Reinbrand","Reinburg","Reinelm","Reinfred","Reingard","Reinger","Reingot","Reinhilde","Reinhoh","Reinlinde","Reinmar","Reinmod","Reinteus","Reintilde","Reintrude","Reinulf","Reinwar","Reinward","Reinwise","Remy","Rene","Renee","Reynard","Reynold","Rhodri","Rhys","Rhyshoiarn","Ribaud","Ribert","Ricaud","Richa","Richard","Richarde","Richberg","Richberga","Richelm","Richer","Richfrid","Richild","Richlinde","Richman","Richmar","Richmund","Richolf","Rigo","Riquin","Ritfrid","Rithilde","Robert","Roberta","Roderick","Rodo","Rodrad","Rodwin","Roenhoiarn","Roenwallon","Roger","Rogera","Rogue","Roland","Roman","Romana","Rosamund","Roscelin","Rosceline","Rose","Rossa","Rosso","Rotbald","Rothard","Rothmund","Rothward","Rubeus","Ruby","Rudolf","Rudolt","Rufus","Rustic","Rustica","Ruth","Rutharda",
"Sabata","Sabato","Sabin","Sabine","Sadrabald","Sadrahar","Sadrilde","Saidra","Saintisme","Salefrid","Salo","Salome","Salvador","Salvia","Salvo","Salvodeus","Samanilde","Samaritana","Sampson","Samuel","Sancta","Sancto","Sano","Santiago","Sapience","Sapphira","Sara","Saracen","Saracena","Saroilde","Sarwin","Sassa","Sasso","Saul","Saulf","Savage","Savaric","Savia","Savius","Savory","Scipio","Sclavo","Scolace","Scott","Scotta","Seaborn","Seafowl","Sebastian","Sebastiana","Second","Seconda","Sedile","Sehild","Seman","Semila","Senior","Seniora","Sennehilda","Senthilde","Seraphim","Serena","Serene","Sergio","Serich","Serlo","Serventa","Setembrina","Severin","Severina","Sforza","Shadrach","Sibald","Sibyl","Sichaus","Sichilde","Siclebald","Siclebalda","Siclebert","Siclefrid","Siclehard","Siclehilde","Sicleholde","Sicleramna","Sicletrude","Sidonia","Sigbald","Sigband","Sigbert","Sigbod","Siger","Sigerich","Sigfrid","Sigga","Siggo","Sighard","Sigmund","Sigrad","Sigrid","Sigwin","Simon","Simone","Sinister","Sisbert","Siserich","Sismund","Sisulf","Sixt","Snorri","Soave","Soliana","Solomon","Solomona","Sonifrida","Sophia","Spania","Stabilia","Stanimir","Stanislav","Steinarr","Steinhard","Stella","Sten","Stephanie","Stephen","Styrbiorn","Sufficia","Sulhoiarn","Sulon","Sulwal","Sulwored","Sumarlidi","Superantia","Supplice","Supplicia","Susan","Suspecta","Svatobor","Sven","Svetoslav","Swanhilde","Sylvester","Sylvestra","Sylvia","Sylvius",
"Tabitha","Tagibod","Tallboys","Tamar","Tancran","Tancred","Tanculf","Tanquard","Tasso","Teague","Tedesca","Tedesco","Temperance","Terence","Thaddeus","Thanburg","Thangmar","Thecla","Theinard","Theinger","Theobald","Theobrand","Theodara","Theodeberg","Theodebert","Theodeberta","Theodebrand","Theodefrid","Theodegarde","Theodegaria","Theodegrim","Theodehard","Theodeher","Theodehilde","Theodelinde","Theodeman","Theodemar","Theodemud","Theodenanda","Theoderam","Theodeswith","Theodora","Theodore","Theodoric","Theodrad","Theodram","Theodulf","Theodwald","Theodward","Theodwig","Theodwin","Theophilus","Theresa","Theuda","Theudo","Thomas","Thomasse","Thor","Thora","Thorberg","Thorbiorn","Thord","Thorfinn","Thorfred","Thorgeirr","Thorgil","Thorkill","Thorsten","Tiberia","Tiberius","Tiffany","Tigernach","Timothy","Titian","Titus","Tobias","Transmundus","Triomer","Trojan","Trutbald","Tuathal","Tudor","Tumidia",
"Uga","Ulbert","Ulfkell","Ulrich","Unica","Uno","Urban","Urgellesa","Uriah","Urith","Uromod","Urraca","Ursa","Ursilda","Urso",
"Valens","Valentina","Valentine","Valerian","Valeriana","Valery","Velislav","Venerio","Ventura","Venture","Venuto","Vera","Verderosa","Vermilia","Vermilius","Victor","Victoria","Victorian","Victorius","Vigil","Vigor","Vigore","Villana","Villano","Vincent","Vincenta","Viola","Violet","Virgil","Virgilia","Viridis","Vita","Vitalis","Vivian","Viviana","Vladislav","Volburg","Volkbert","Volkdag","Volkiva","Volkmar","Volkold","Volkrad","Volkrada","Volkran","Volkrich","Volkwara","Volkward","Volkwin","Volswinde","Vrolijk","Vuteria",
"Wace","Wacher","Walahilde","Walantrude","Walateus","Walbert","Walburg","Walcher","Walda","Waldefrid","Waldegaud","Waldeger","Waldegilde","Waldehilde","Waldgaud","Waldo","Waldred","Waldrich","Waldswind","Walerard","Wallon","Walo","Walram","Walrich","Waltbert","Walter","Waltera","Waltger","Walthad","Walthard","Waltilde","Waltman","Wanegar","Wantelm","Wanthilde","Warin","Warina","Warmund","Warnbald","Warnbert","Warner","Warnfrid","Warnger","Warnhard","Warnulf","Wasmod","Wedekind","Weltrude","Wenceslas","Wendel","Wendela","Wendelbert","Wendelburg","Wendelfrid","Wendelgard","Wendelgaud","Wendelger","Wendelmar","Wendelmoet","Wendelswith","Werrich","Werwald","Whitehelm","Wichard","Wigand","Wigbald","Wigbert","Wigelm","Wigfrid","Wiggo","Wigher","Wigmar","Wilbert","Wilbrand","Wilenc","Wilfred","Wilher","Wilhilde","Wilk","Willard","Willberna","Willegis","Willelma","William","William-Geoffrey","William-Raymond","Willo","Willulf","Wilrich","Wina","Winand","Wine","Winebald","Wineberg","Winebert","Winegard","Winegarde","Winegilde","Winegis","Winegod","Wineman","Winemund","Winerad","Wineran","Winfred","Winifred","Winmar","Winrich","Winsy","Wintbald","Wintbert","Winter","Winulf","Wirich","Wiseltrude","Wistrilde","Witoslav","Witta","Witugis","Wojciech","Wojslav","Wolf","Wolfald","Wolfbern","Wolfbert","Wolfer","Wolfgang","Wolfgrim","Wolfgunda","Wolfhart","Wolfheah","Wolfhelm","Wolfram","Wolschalk","Wortwin","Wulfbald","Wulfbrand","Wulfgarde","Wulfger","Wulfgis","Wulfred","Wulfrich","Wulfstan","Wulfsy","Wulftrude","Wulviva","Wyburg",
"Yael","Yolanda","Ysoria",
"Zacchaeus","Zacharia","Zachary","Zawissius","Zbincza","Zbygniew","Zdeslav","Zdeslava","Zemislav","Zenobius","Zipporah","Zoete","Zwentibold"
    };

    
    public class Save
    {
        public string name;
        public byte level;
        public byte unassigned;
        public byte wind;
        public byte ice;
        public byte fire;
    }

    public static Save currentSave = null;

    
    public static Save save1;
    public static Save save2;
    public static Save save3;

    public static bool aSaveExists;
    public static bool savesFull;

    private static readonly string SaveDir = "/";
    private static readonly string DotJSON = ".json";
    

    private static string getFullPath(string filename)
    {
        return Application.persistentDataPath + SaveDir + filename + DotJSON;
    }

    private static bool LoadSaveFile(string filename, ref Save save)
    {
        string fullPath = getFullPath(filename);
        if (File.Exists(fullPath))
        {
            StreamReader reader = new StreamReader(fullPath);
            string rawJSON = reader.ReadToEnd();
            save = JsonUtility.FromJson<Save>(rawJSON);
            aSaveExists = true;
            reader.Close();
            return true;
        }
        return false;
    }

    public static void LoadSaveFiles()
    {
        aSaveExists = false;
        savesFull = true;
        savesFull &= LoadSaveFile("save1", ref save1);
        savesFull &= LoadSaveFile("save2", ref save2);
        savesFull &= LoadSaveFile("save3", ref save3);
    }

    public static void CreateSave1(string name)
    {
        save1 = createNewSave(name);
    }

    public static void CreateSave2(string name)
    {
        save2 = createNewSave(name);
    }

    public static void CreateSave3(string name)
    {
        save3 = createNewSave(name);
    }

    private static Save createNewSave(string name)
    {
        Save save = new Save();
        save.name = name;
        save.level = 1;
        save.unassigned = 1;
        save.wind = 0;
        save.ice = 0;
        save.fire = 0;
        
        return save;
    }

    public static void SaveAllFiles()
    {
        DebugSave();
        SaveAsJSON(save1, "save1");
        SaveAsJSON(save2, "save2");
        SaveAsJSON(save3, "save3");
    }

    private static void SaveAsJSON(Save toSave, string filename)
    {
        if (toSave == null)
        {
            Debug.Log("not a valid save aborting");
            return;
        }
        DebugSpecificSave(toSave);
        string rawJSON = JsonUtility.ToJson(toSave);

        Debug.Log("writing tp path: " + getFullPath(filename));
        
        using (TextWriter writer = new StreamWriter(getFullPath(filename), false))
        {
            writer.WriteLine(rawJSON);
            writer.Close();
        }

        
        
        //File.WriteAllText(getFullPath(filename), rawJSON);
        
    }

    public static void DebugSave()
    {
        DebugSpecificSave(currentSave);
    }

    public static void DebugSpecificSave(Save save)
    {
        Debug.Log("current save: " + save.name + "\n\tunassigned=" + save.unassigned + "\n\twind=" + save.wind + "\n\tice=" + save.ice + "\n\tfire=" + save.fire);
    }

    public static string GenerateName()
    {
        return names[UnityEngine.Random.Range(0, names.Length)];
    }
}
