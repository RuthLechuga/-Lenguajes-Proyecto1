using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto1.Clases
{ 
    class analisisLexico
    {
        //DECLARACIÓN LISTAS
        List<Token> tokens;
        List<Errores> erroresLexicos;
        List<String> palabrasReservadas;

        //DECLARACIÓN VARIABLES
        String texto;
        int fila;
        int columna;
        int estado;
        String auxLex;
        char c;
        Boolean punto;

        public analisisLexico()
        {
            //INICIALIZACIÓN VARIABLES
            tokens = new List<Token>();
            erroresLexicos = new List<Errores>();
            palabrasReservadas = new List<String>();

            fila = 1;
            columna = 1;
            estado = 0;
            auxLex = "";
            c = (char)00;
            punto = false;

            añadirPalabrasReservadas();
        }

        public List<Token> realizarAnalisis(String texto)
        {
            this.texto = texto;

            for(int i = 0; i < texto.Length; i++)
            {
                c = texto[i];
                switch (estado)
                {
                    case 0:
                        columna++;
                        if (Char.IsLetter(c))
                        {
                            estado = 1;
                            auxLex += c;
                        }
                        else if (Char.IsDigit(c))
                        {
                            estado = 2;
                            auxLex += c;
                        }
                        else if (c == '+')
                            agregarToken("+", "Signo Suma", 3);
                        else if (c == '-')
                            agregarToken("-", "Signo Menos", 4);
                        else if (c == '*')
                            agregarToken("*", "Signo Multiplicar", 5);
                        else if (c == '/')
                            agregarToken("/", "Diagonal", 6);
                        else if (c == '^')
                            agregarToken("^", "Elevar", 7);
                        else if (c == '=')
                            agregarToken("=", "Signo Igual", 8);
                        else if (c == ';')
                            agregarToken(";", "Fin de comando", 9);
                        else if (c == ',')
                            agregarToken(",", "Coma", 10);
                        else if (c == '(')
                            agregarToken("(", "Parentesis Izquierdo", 11);
                        else if (c == ')')
                            agregarToken(")", "Parentesis derecho", 12);
                        else if (c == (char)10)
                        {
                            fila++;
                            columna = 1;
                        } else if (c == ' ');
                        else
                        {
                            erroresLexicos.Add(new Errores(erroresLexicos.Count + 1, fila, columna, c.ToString(), "Desconocido"));
                        }
                        break;

                    case 1:
                        if (Char.IsLetterOrDigit(c) || c == '_')
                        {
                            auxLex += c;
                            columna++;
                        }
                        else
                        {
                            if (esReservada(auxLex))
                                agregarToken(auxLex, "Palabra Reservada", 1);
                            else
                                agregarToken(auxLex, "Palabra Desconocida", 2);

                            estado = 0;
                            i--;
                            auxLex = "";
                        }
                        break;

                    case 2:
                        if (Char.IsDigit(c))
                        {
                            auxLex += c;
                            columna++;
                        }
                        else if(c == '.' && !punto)
                        {
                            auxLex += c;
                            punto = true;
                        }
                        else 
                        {
                            agregarToken(auxLex, "Número", 13);
                            punto = false;
                            estado = 0;
                            i--;
                            auxLex = "";
                        }
                        break;
                }
            }
            return tokens;
        }

        public Boolean Comparar(String cadena1, String cadena2)
        {
            if (String.Compare(cadena1, cadena2, true) == 0)
                return true;
            else
                return false;
        }

        public Boolean esReservada(String palabra)
        {
            Boolean bandera = false;
            foreach (String elemento in palabrasReservadas)
            {
                if (Comparar(elemento,palabra))
                {
                    bandera = true;
                }
            }
            return bandera;
        }

        public void agregarToken(String lexema, String tipo, int id_tipo)
        {
            tokens.Add(new Token(lexema, fila, columna, id_tipo, tipo));
        }

        public List<Errores> obtenerErroresLexicos()
        {
            return erroresLexicos;
        }

        private void añadirPalabrasReservadas()
        {
            palabrasReservadas.Add("asg");
            palabrasReservadas.Add("Escribir");
            palabrasReservadas.Add("TamañoLienzo");
            palabrasReservadas.Add("Tl");
            palabrasReservadas.Add("ColorLienzo");
            palabrasReservadas.Add("Cl");
            palabrasReservadas.Add("avanzar");
            palabrasReservadas.Add("avz");
            palabrasReservadas.Add("retroceder");
            palabrasReservadas.Add("ret");
            palabrasReservadas.Add("girarIzq");
            palabrasReservadas.Add("izq");
            palabrasReservadas.Add("girarDer");
            palabrasReservadas.Add("Der");
            palabrasReservadas.Add("centrar");
            palabrasReservadas.Add("ir");
            palabrasReservadas.Add("irX");
            palabrasReservadas.Add("ix");
            palabrasReservadas.Add("irY");
            palabrasReservadas.Add("iy");
            palabrasReservadas.Add("subirPincel");
            palabrasReservadas.Add("spl");
            palabrasReservadas.Add("bajarPincel");
            palabrasReservadas.Add("bpl");
            palabrasReservadas.Add("colorPincel");
            palabrasReservadas.Add("cpl");
            palabrasReservadas.Add("blanco");
            palabrasReservadas.Add("celeste");
            palabrasReservadas.Add("amarillo");
            palabrasReservadas.Add("rojo");
            palabrasReservadas.Add("azul");
            palabrasReservadas.Add("negro");
        }
    }
}
