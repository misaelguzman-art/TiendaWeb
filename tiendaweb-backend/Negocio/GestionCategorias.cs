public class GestionCategorias
{
    public List<Categoria> ListaCategorias()
    {
        var result = new List<Categoria>
        {
             new() {Id =1 ,Nombre = "Hardware",Descripcion = "Todo lo relacionado con hardware"},
             new() {Id =2 ,Nombre = "Software",Descripcion = "Todo lo relacionado con software"},
             new() {Id =3 ,Nombre = "Perifericos",Descripcion = "Todo lo relacionado con perifericos"}
        };

        return result;
    }


}