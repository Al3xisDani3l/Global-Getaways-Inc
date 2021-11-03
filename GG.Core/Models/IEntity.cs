using System;



namespace GG.Core
{
    /// <summary>
    /// Define las propiedades basicas de identificacion en una base de datos relacional.
    /// </summary>
    public interface IEntity<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Id unico entero de identificacion
        /// </summary>
        public T Id { get; set; }
        /// <summary>
        /// Guid de identificacion global, implentacion para bases de datos locales.
        /// </summary>
        public Guid Guid { get; set; }


    }


    public interface IEntity : IEntity<int>
    {

    }
}
