using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Clases
{

    class Variables
    {
        String id;
        Object valor;

        public Variables(String id, Object valor)
        {
            this.id = id;
            this.valor = valor;
        }

        public Variables(String id)
        {
            this.id = id;
            this.valor = "#";
        }

        public string Id { get => id; set => id = value; }
        public object Valor { get => valor; set => valor = value; }
    }
}
