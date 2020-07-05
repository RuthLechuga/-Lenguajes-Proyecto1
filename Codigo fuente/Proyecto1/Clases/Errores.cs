using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Clases
{
    class Errores
    {
        int id;
        int fila;
        int columna;
        String palabra;
        String descripcion;

        public Errores(int id, int fila, int columna, String palabra, String descripcion)
        {
            this.id = id;
            this.fila = fila;
            this.columna = columna;
            this.palabra = palabra;
            this.descripcion = descripcion;
        }

        public int Id { get => id; set => id = value; }
        public int Fila { get => fila; set => fila = value; }
        public int Columna { get => columna; set => columna = value; }
        public String Palabra { get => palabra; set => palabra = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
    }
}
