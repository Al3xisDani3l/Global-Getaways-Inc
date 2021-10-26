using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GG.Core;
using AutoMapper;

namespace GG.Api.Automapper
{
    /// <summary>
    /// Configura a Automapper de manera automatica
    /// </summary>
    public class AutomapperInitializer : Profile
    {

        /// <summary>
        /// Configura a Automapper de manera automatica
        /// </summary>
        public AutomapperInitializer(AutomapperProfile profile)
        {
            Map(profile);
        }

        internal void Map(AutomapperProfile profile)
        {

            //mapeamos primero los tipos de aquellos objetos pertenecientes al ensamblado
            //iteramos cada uno de los elementos de el diccionario y obtenemos su tipo
            if (profile.FromTo is not null)
            {
                foreach (var valuePair in profile.FromTo)
                {

                    Type to = Type.GetType($"{profile.Namespace}.{valuePair.Key}, {profile.Namespace}");
                    Type from = Type.GetType($"{profile.Namespace}.{valuePair.Value}, {profile.Namespace}");
                    //comprobamos que existan las clases
                    if (to is not null && from is not null)
                    {
                        CreateMap(to, from);
                        CreateMap(from, to);
                    }

                }
            }

            //mapeamos los casos especiales donde no hay uniformidad con el ensamblado

            if (profile.SpecialFromTo is not null)
            {
                foreach (var valuePair in profile.SpecialFromTo)
                {

                    Type to = Type.GetType(valuePair.Key);
                    Type from = Type.GetType(valuePair.Value);
                    //comprobamos que existan las clases
                    if (to is not null && from is not null)
                    {
                        CreateMap(to, from);
                        CreateMap(from, to);
                    }

                }
            }

        }


    }
}
