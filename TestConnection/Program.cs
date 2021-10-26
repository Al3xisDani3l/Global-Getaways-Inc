using System;
using System.Threading.Tasks;
using GG.Infrastructure.Data;
using Npgsql;
using GG.Core.Entities;
using GG.Core.Services;
using System.Security.Cryptography;
using GG.Core.Validations;
using Newtonsoft.Json;

namespace TestConnection
{
    class Program
    {

        public static string Hash(string password)
        {
            //PBKDF2 implementation
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                16,
                10000
                ))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(32));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{10000}.{salt}.{key}";
            }
        }


       

        static void Main(string[] args)
        {

            Usuario usuario = new Usuario();

            NotNullValidation<Usuario> notNull = new NotNullValidation<Usuario>();

            Func<Orden, bool> negativos = o =>  o is null?true:o.Alto >=0 ;
            Func<Orden, bool> negativos2 = o => o is null ? true : o.Ancho >= 0;
            Func<Orden, bool> fecha = o => o is null ? true : o.FechaRealizacionDeseada > DateTime.Now;
           



            var result = JsonConvert.SerializeObject(notNull, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }) ;

            BaseValidationsGeneric<Usuario> test = JsonConvert.DeserializeObject<BaseValidationsGeneric<Usuario>>(result);

             var nofuenulo =  test.Validation.Invoke(usuario);


            Console.WriteLine(result);



            
            //NpgsqlConnectionStringBuilder connstring = new NpgsqlConnectionStringBuilder();

            ////"Server:.;Port:5432;Database:GGdatabase;User Id:postgres;Password:GG12345;"
            ////Server=ec2-34-193-113-223.compute-1.amazonaws.com;Port=5432;Database=depsjd3oarplor;User Id=yhuwxgiqefjtkd;Password=ff0cc7ca638abcddb64a97750050e1ab910a97b6ac1ffca2a4155fceebb1cbcd;sslmode=Require;Trust Server Certificate=true;
            //connstring.Database = "depsjd3oarplor";
            //connstring.Password = "ff0cc7ca638abcddb64a97750050e1ab910a97b6ac1ffca2a4155fceebb1cbcd";
            //connstring.Username = "yhuwxgiqefjtkd";
            //connstring.Host = "ec2-34-193-113-223.compute-1.amazonaws.com";
        
            //connstring.Port = 5432;



            //GGDBContext conn = new GGDBContext();
           

            //try
            //{


            //    Usuario administer = new Usuario();

            //    administer.Email = @"GG_api@outlook.com";
            //    administer.Telefono = "6562538679";
            //    administer.Nombre = "GGadmin";
            //    administer.FechaNacimiento = DateTime.Now;
            //    administer.Apellido = "Principal";
            //    administer.Password = Hash("GG12345");
            //    administer.Role = GG.Core.Enumerations.RoleType.Administrator;
            //    administer.GoogleUUID = null;
            //    administer.Suscrito = false;
               


            //    if (conn.Database.CanConnect())
            //    {
            //        conn.Usuarios.Add(administer);
            //       Console.WriteLine("Se ha agredio {0} usuarios a la db", conn.SaveChanges());
            //    }
            //    else
            //    {
            //        Console.WriteLine("No se pudo conectar a la base de datos");
            //    }


            //    //Console.WriteLine(connstring.ConnectionString);

            //    //NpgsqlConnection npgconn = new NpgsqlConnection(connstring.ConnectionString);

            //    //npgconn.Open();

            //    //npgconn.Close();
            //    //Console.WriteLine(conn.Database.ProviderName);
            //    //Console.WriteLine("EL contexto se pudo conectar: {0}", conn.Database.CanConnect());

            //    //var resultado = conn.Database.EnsureCreated();
            //    //Console.WriteLine(resultado);

            //}
            //catch (Exception err)
            //{

            //    Console.WriteLine(err);
            //}
 Console.WriteLine("Hello World!");
        }
    }
}
