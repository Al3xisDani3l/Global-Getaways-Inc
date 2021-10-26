using System;



namespace GG.Core
{
    /// <summary>
    /// Define las propiedades basicas de identificacion en una base de datos relacional.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Id unico entero de idetificacion
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Guid de identificacion global, implentacion para bases de datos locales.
        /// </summary>
        public Guid Guid { get; set; }


    }
}
