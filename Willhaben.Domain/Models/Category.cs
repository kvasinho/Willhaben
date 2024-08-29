using Willhaben.Domain.Exceptions;
using System.Collections.Generic;


namespace Willhaben.Domain.Models;



public class Category
{
    private string? _selectedCategory;

    public string? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (value != null && Categories.TryGetValue(value, out string categoryCode))
            {
                _selectedCategory = categoryCode;
            }
            else
            {
                throw new InvalidCategoryException(value ?? "");
            }
        }
    }


    public static Dictionary<string, string> Categories = new Dictionary<string, string>
    {
        {"sportgeräte", "sport-sportgeraete-4390"},
            {"angeljagd", "angelsport-jagdsport-4391"},
                {"angeljagd-angelausrüstung", "angelsport-jagdsport/angelausruestung-4392"},
                    {"angeljagd-angelausrüstung-angelhaken", "angelausruestung/angelhaken-gewichte-4393"},
                    {"angeljagd-angelausrüstung-angelköder", "angelausruestung/angelkoeder-futtermittel-4394"},
                    {"angeljagd-angelausrüstung-angelschnur", "angelausruestung/angelrollen-angelschnur-4395"},
                    {"angeljagd-angelausrüstung-angelruten", "angelausruestung/angelruten-4396"},
                    {"angeljagd-angelausrüstung-angelsets", "angelausruestung/angel-sets-4397"},
                    {"angeljagd-angelausrüstung-kescher", "angelausruestung/kescher-4398"},
                    {"angeljagd-angelausrüstung-schwimmer", "angelausruestung/schwimmer-4399"},
                {"angeljagd-jagdbekleidung", "angelsport-jagdsport/angel-jagdbekleidung"},
                {"angeljagd-fernglas", "angelsport-jagdsport/fernglaeser-optik-4401"},
                {"angeljagd-geweihetierpräparate", "angelsport-jagdsport/geweihe-tierpraeparate-4402"},
                    {"angeljagd-geweihetierpräparate-geweihe", "geweihe-tierpraeparate/geweihe-4403"},
                    {"angeljagd-geweihetierpräparate-tierpräparate", "geweihe-tierpraeparate/tierpraeparate-felle-4404"},
                {"angeljagd-jagsausrüstung", "angelsport-jagdsport/jagdausruestung-4405"},
                {"angeljagd-jagdmesser", "angelsport-jagdsport/jagdmesser-taschenmesser-4406"},
                {"angeljagd-angeltaschen", "angelsport-jagdsport/angeltaschen-koffer-4407"},
            {"ballsport", "ballsportarten-4408"},
                {"football", "ballsportarten/american-football-rugby-4409"},
                {"baseball", "ballsportarten/baseball-4410"},
                {"basketball", "ballsportarten/basketball-4411"},
                {"fußball", "ballsportarten/fussball-4412"},
                    {"fußball-bekleidung", "fussball/fussballbekleidung-4413"},
                        {"fußball-bekleidung-accessoires", "fussballbekleidung/accessoires-4414"},
                        {"fußball-bekleidung-hosen", "fussballbekleidung/fussballhosen-4415"},
                        {"fußball-bekleidung-jacken", "fussballbekleidung/fussballjacken-4416"},
                        {"fußball-bekleidung-pullover", "fussballbekleidung/fussballpullover-westen-4417"},
                        {"fußball-bekleidung-shorts", "fussballbekleidung/fussballshorts-4418"},
                        {"fußball-bekleidung-socken", "fussballbekleidung/fussballsocken-stutzen-4419"},
                        {"fußball-bekleidung-trickots", "fussballbekleidung/fussballtrikots-shirts-4420"},
                    {"fußball-bekleidung-kinder", "fussball/fussballbekleidung-kinder-4421"},
                        {"fußball-bekleidung-kinder-accessoires", "fussballbekleidung-kinder/accessoires-4422"},
                        {"fußball-bekleidung-kinder-hosen", "fussballbekleidung-kinder/fussballhosen-4423"},
                        {"fußball-bekleidung-kinder-jacken", "fussballbekleidung-kinder/fussballjacken-4424"},
                        {"fußball-bekleidung-kinder-pullover", "fussballbekleidung-kinder/fussballpullover-westen-4425"},
                        {"fußball-bekleidung-kinder-shorts", "fussballbekleidung-kinder/fussballshorts-4426"},
                        {"fußball-bekleidung-kinder-socken", "fussballbekleidung-kinder/fussballsocken-stutzen-4427"},
                        {"fußball-bekleidung-kinder-trickots", "fussballbekleidung-kinder/fussballtrikots-shirts-4428"},
                    {"fußball-fußbälle", "fussball/fussbaelle-tore-4429"},
                    {"fußball-knieschützer", "fussball/knieschuetzer-protektoren-4430"},
                    {"fußball-schuhe", "fussball/schuhe-4431"},
                        {"fußball-schuhe-fußballschuhe", "schuhe/fussballschuhe-4432"},
                        {"fußball-schuhe-hallenschuhe", "schuhe/hallenschuhe-4433"},
                    {"fußball-taschen", "fussball/taschen-4434"},
                    //TODO: Ballsportarten
                    //TODO: ANDERE SPORTARTEN
                    {"radsport", "fahrraeder-radsport-4525"},
                        {"radsport-bekleidung", "fahrraeder-radsport/fahrradbekleidung-4526"},
                            {"radsport-bekleidung-accessoires", "fahrradbekleidung/accessoires-4527"},
                            {"radsport-bekleidung-handschuhe", "fahrradbekleidung/fahrradhandschuhe-4528"},
                            {"radsport-bekleidung-hosen", "fahrradbekleidung/fahrradhosen-4529"},
                            {"radsport-bekleidung-jacken", "fahrradbekleidung/fahrradjacken-4530"},
                            {"radsport-bekleidung-protektoren", "fahrradbekleidung/fahrrad-protektoren-4531"},
                            {"radsport-bekleidung-shirts", "fahrradbekleidung/fahrradtrikots-shirts-4532"},
                            {"radsport-bekleidung-socken", "fahrradbekleidung/waesche-fahrradsocken-4533"},
                        {"radsport-bekleidung-kinder", "fahrraeder-radsport/fahrradbekleidung-kinder-4534"},
                            {"radsport-bekleidung-kinder-accessoires", "fahrradbekleidung-kinder/accessoires-4535"},
                            {"radsport-bekleidung-kinder-handschuhe", "fahrradbekleidung-kinder/fahrradhandschuhe-4536"},
                            {"radsport-bekleidung-kinder-hosen", "fahrradbekleidung-kinder/fahrradhosen-4537"},
                            {"radsport-bekleidung-kinder-jacken", "fahrradbekleidung-kinder/fahrradjacken-4538"},
                            {"radsport-bekleidung-kinder-protektoren", "fahrradbekleidung-kinder/fahrrad-protektoren-4539"},
                            {"radsport-bekleidung-kinder-shirts", "fahrradbekleidung-kinder/fahrradtrikots-shirts-4540"},
                            {"radsport-bekleidung-kinder-socken", "fahrradbekleidung-kinder/waesche-fahrradsocken-4541"},
                        {"radsport-ersatzteile", "fahrraeder-radsport/fahrrad-ersatzteile-zubehoer-4542"},
                            {"radsport-ersatzteile-aufbewahrung", "fahrrad-ersatzteile-zubehoer/aufbewahrung-7236"},
                            {"radsport-ersatzteile-beleuchtung", "ahrrad-ersatzteile-zubehoer/beleuchtung-sicherheit-7237"},
                            {"radsport-ersatzteile-bremsen", "fahrrad-ersatzteile-zubehoer/bremsen-4543"},
                            {"radsport-ersatzteile-ebatterien", "fahrrad-ersatzteile-zubehoer/e-bike-batterien-zubehoer-7238"},
                            {"radsport-ersatzteile-gepäckträgertaschen", "fahrrad-ersatzteile-zubehoer/gepaecktraeger-taschen-7239"},
                            {"radsport-ersatzteile-kettenblätterschaltungen", "fahrrad-ersatzteile-zubehoer/kettenblaetter-ketten-schaltungen-7240"},
                            {"radsport-ersatzteile-luftpumpen", "fahrrad-ersatzteile-zubehoer/luftpumpen-7241"},
                            {"radsport-ersatzteile-pedale", "fahrrad-ersatzteile-zubehoer/pedale-4545"},
                            {"radsport-ersatzteile-radcomputersensoren", "fahrrad-ersatzteile-zubehoer/radcomputer-sensoren-4546"},
                            {"radsport-ersatzteile-rahmengabelnlenker", "fahrrad-ersatzteile-zubehoer/rahmen-gabeln-lenker-4547"},
                            {"radsport-ersatzteile-reifenfelgenschläuche", "fahrrad-ersatzteile-zubehoer/reifen-felgen-schlaeuche-4548"},
                            {"radsport-ersatzteile-sättel", "fahrrad-ersatzteile-zubehoer/saettel-4549"},
                        {"radsport-anhänger", "fahrraeder-radsport/fahrradanhaenger-4550"},
                        {"radsport-schuhe", "fahrraeder-radsport/fahrradschuhe-4551"},
                        {"radsport-helme", "fahrraeder-radsport/fahrradhelme-4564"},
                        {"radsport-kindersitze", "kindersitze-4565"},
                        {"radsport-fahrrad", "fahrraeder-radsport/fahrraeder-4552"},
                            {"radsport-fahrrad-bmx", "fahrraeder/bmx-4553"},
                            {"radsport-fahrrad-citybike", "fahrraeder/citybikes-stadtraeder-4554"},
                            {"radsport-fahrrad-cruiser", "fahrraeder/cruiser-4555"},
                            {"radsport-fahrrad-ebike", "fahrraeder/e-bikes-4556"},
                            {"radsport-fahrrad-einrad", "fahrraeder/einraeder-4557"},
                            {"radsport-fahrrad-gravel", "fahrraeder/gravel-bikes-5010050"},
                            {"radsport-fahrrad-kinderrad", "fahrraeder/kinderfahrraeder-4558"},
                            {"radsport-fahrrad-mountainbike", "fahrraeder/mountainbikes-4559"},
                            {"radsport-fahrrad-rennrad", "fahrraeder/rennraeder-4560"},
                            {"radsport-fahrrad-fixie", "fahrraeder/singlespeed-fixies-4561"},
                            {"radsport-fahrrad-trekking", "fahrraeder/trekkingfahrraeder-crossbikes-4562"},
                            {"radsport-fahrrad-triathlon", "fahrraeder/triathlon-zeitfahrraeder-4563"},
                            





                        




                        


                    
                    
                    
                        
                    
                    


        
                
                
                
                


                
            
                
                    
                
    };

}


