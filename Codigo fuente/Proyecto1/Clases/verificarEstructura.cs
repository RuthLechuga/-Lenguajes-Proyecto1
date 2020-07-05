using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto1.Clases
{
    class verificarEstructura
    {
        List<Token> lista;
        List<String> errores;
        List<Variables> variables;
        List<String> listaSalida;
        List<String> pila;

        int estado;
        Boolean parentesis;
        Boolean existeVariable;
        Boolean variableExistente;

        public verificarEstructura()
        {
            lista = new List<Token>();
            errores = new List<String>();
            variables = new List<Variables>();
            listaSalida = new List<String>();
            pila = new List<String>();

            parentesis = false;
            existeVariable = false;
            variableExistente = false;
        }

        public List<String> validarEstructura(List<Token> tokens)
        {
            this.lista = tokens;
            String nombre = " ";
            int tipo;
            int pos;

            for (int i = 0; i < lista.Count && errores.Count == 0; i++)
            {
                estado = 0;
                pos = 0;
                variableExistente = false;
                listaSalida.Clear();
                pila.Clear();

                //VERIFICANDO SI ES VARIABLE Y YA EXISTE
                for (int j =0;j<variables.Count && !variableExistente;j++)
                    if (variables.ElementAt(j).Id == lista.ElementAt(i).Lexema)
                    {
                        variableExistente = true;
                        pos = j;
                    }

                //ESTRUCTURA DE DECLARACIÓN DE VARIABLES
                if (Comparar("asg", lista.ElementAt(i).Lexema) || (variableExistente && lista.ElementAt(i+1).Lexema == "="))
                {
                    i++;
                    if (variableExistente) i--;

                    while (i < lista.Count && lista.ElementAt(i).IdToken != 9 && errores.Count == 0)
                    {
                        tipo = lista.ElementAt(i).IdToken;
                        switch (estado)
                        {
                            case 0:
                                if (tipo == 2)
                                {
                                    estado = 1;
                                    nombre = lista.ElementAt(i).Lexema;
                                }
                                else errores.Add("Se esperaba un identificador de variable");
                                break;
                            case 1:
                                if (tipo == 8)
                                    estado = 2;
                                else errores.Add("Se esperaba una igualacion");
                                break;
                            case 2:
                                posfija(tipo, lista.ElementAt(i).Lexema);
                                break;
                        }
                        i++;
                    }
                    while (pila.Count > 0 && (pila.ElementAt(pila.Count - 1) != "("))
                    {
                        listaSalida.Add(pila.ElementAt(pila.Count - 1));
                        pila.RemoveAt(pila.Count - 1);
                    }

                    if (!variableExistente)
                    {
                        if (pila.Count == 0 && listaSalida.Count > 0)
                            variables.Add(new Variables(nombre, realizarOperacion(listaSalida)));
                        else if (listaSalida.Count == 0)
                            variables.Add(new Variables(nombre));
                    }
                    else 
                        variables.ElementAt(pos).Valor = realizarOperacion(listaSalida);
                }
            }

            foreach (Variables variable in variables)
                Console.WriteLine(variable.Id + "=" + variable.Valor.ToString() );

            return errores;
        }

        public void posfija(int tipo, String valor)
        {
            //si es variable
            if (tipo == 2)
            {
                foreach (Variables variable in variables)
                    if (variable.Id == valor)
                    {
                        if (variable.Valor.ToString() == "#")
                            errores.Add("La variable: " + variable.Id + "no tiene valor asignado");
                        else
                            listaSalida.Add(variable.Valor.ToString());

                        existeVariable = true;
                    }

                if (!existeVariable)
                    errores.Add("La variable no ha sido declarada");

                parentesis = false;
            }

            //si es numero
            else if (tipo == 13)
                listaSalida.Add(valor);

            //si es operador
            else if (tipo > 2 && tipo < 8)
            {
                if (parentesis && tipo == 4 || (tipo==4 && listaSalida.Count==0))
                    listaSalida.Add("0");
                while (pila.Count > 0 && (jerarquia(valor) <= jerarquia(pila.ElementAt(pila.Count - 1))))
                {
                    listaSalida.Add(pila.ElementAt(pila.Count - 1));
                    pila.RemoveAt(pila.Count - 1);
                }
                pila.Add(valor);

                parentesis = false;
            }

            //si es parentesis izquierdo
            else if (tipo == 11)
            {
                parentesis = true;
                pila.Add(valor);
            }

            //si es parentesis derecho
            else if (tipo == 12)
            {
                while (pila.Count > 0 && (pila.ElementAt(pila.Count - 1) != "("))
                {
                    listaSalida.Add(pila.ElementAt(pila.Count - 1));
                    pila.RemoveAt(pila.Count - 1);
                }
                pila.RemoveAt(pila.Count - 1);
            }

            else errores.Add("¡Error encontrada dentro de la operación");
        }

        private double realizarOperacion(List<string> operacion)
        {
            List<Double> operar = new List<Double>();
            double valor1; double valor2;

            foreach(String elemento in operacion)
            {
                if(elemento == "*" || elemento == "+" || elemento == "-" || elemento == "/" || elemento == "^")
                {
                    valor1 = operar.ElementAt(operar.Count - 1);
                    valor2 = operar.ElementAt(operar.Count - 2);
                    operar.RemoveAt(operar.Count - 1);
                    operar.RemoveAt(operar.Count - 1);

                    if (elemento == "*") operar.Add(valor1 * valor2);
                    if (elemento == "+") operar.Add(valor1 + valor2);
                    if (elemento == "/") operar.Add(valor2 / valor1);
                    if (elemento == "-") operar.Add(valor2 - valor1);
                    if (elemento == "^") operar.Add(Math.Pow(valor2, valor1));

                } else
                {
                    operar.Add(Convert.ToDouble(elemento));
                }
                    
            }
            return operar.ElementAt(0);
        }

        public int jerarquia(String c)
        {
            int jer = 100;

            switch (c)
            {
                case "+":
                case "-": jer = 1;
                    break;
                case "*":
                case "/": jer = 2;
                    break;
                case "^": jer = 3;
                    break;
                default: jer = 0;
                    break;
            }
            return jer;
        }

        public Boolean Comparar(String cadena1, String cadena2)
        {
            if (String.Compare(cadena1, cadena2, true) == 0)
                return true;
            else
                return false;
        }

        public List<Variables> obtenerVariables()
        {
            return variables;
        }

    }
}
