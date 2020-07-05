using Proyecto1.Clases;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto1
{
    public partial class Form1 : Form
    {
        Bitmap b;
        Bitmap Imagen;
        Pen pen;
        Graphics g;
        Double tamañox;
        Double tamañoy;
        int posXT;
        int posYT;
        int posix;
        int posiy;
        double angulo;
        double anguloT;
        Boolean bajarLapiz;
        String ruta;

        List<Token> tokens;
        List<Errores> errores;
        List<Variables> variables;

        public Form1()
        {
            InitializeComponent();

            restablecerHerramientas();

            //INICIALIZANDO VARIABLES
            tokens = new List<Token>();
            errores = new List<Errores>();
            variables = new List<Variables>();
            angulo = 90;
            anguloT = 0;
            ruta = "";

            Imagen = new Bitmap("tortuga2.png", true);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analisisLexico analizador = new analisisLexico();
            tokens = analizador.realizarAnalisis(editor.Text);
            errores = analizador.obtenerErroresLexicos();

            if(errores.Count == 0)
            {
                verificarEstructura verificador = new verificarEstructura();
                verificador.validarEstructura(tokens);
                variables = verificador.obtenerVariables();

                realizarFunciones();
            } else
            {
                MessageBox.Show("Hay errores léxicos");
            }
        }

        private void realizarFunciones()
        {
            String funcion;
            restablecerHerramientas();
            consola.Text = "";

            for(int i=0; i<tokens.Count;i++)
            {
                funcion = tokens.ElementAt(i).Lexema;
               // MessageBox.Show("");

                if (Comparar(funcion, "escribir"))
                {
                    consola.Text += tokens.ElementAt(i + 2).Lexema + "=" + obtenerValorVar(tokens.ElementAt(i + 2).Lexema) + "\n";
                    i = i + 3;
                }

                else if (Comparar(funcion, "colorlienzo") || Comparar(funcion, "cl"))
                {
                    lienzo.BackColor = obtenerColorLienzo(tokens.ElementAt(i + 1).Lexema);
                    lienzo.Refresh();
                    i = i + 2;
                }

                else if (Comparar(funcion, "colorpincel") || Comparar(funcion, "cpl"))
                {
                    pen.Color = obtenerColorPincel(tokens.ElementAt(i + 1).Lexema);
                    i = i + 2;
                }

                else if (Comparar(funcion, "tamañoLienzo") || Comparar(funcion, "tl"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                        tamañox = Convert.ToDouble(tokens.ElementAt(i + 1).Lexema);
                    else
                        tamañox = obtenerValorVar(tokens.ElementAt(i + 1).Lexema);

                    if (tokens.ElementAt(i + 3).IdToken == 13)
                        tamañoy = Convert.ToDouble(tokens.ElementAt(i + 3).Lexema);
                    else
                        tamañoy = obtenerValorVar(tokens.ElementAt(i + 3).Lexema);

                    tamañoLienzo(Convert.ToInt32(tamañox), Convert.ToInt32(tamañoy));
                    posix = Convert.ToInt32(lienzo.Width / 2 - tamañox / 2);
                    posiy = Convert.ToInt32(lienzo.Height / 2 - tamañoy / 2);
                    i = i + 4;
                }

                else if (Comparar(funcion, "centrar"))
                {
                    posXT = lienzo.Width / 2;
                    posYT = lienzo.Height / 2;
                    tortuga.Location = new Point(posXT-tortuga.Width/2, posYT-tortuga.Height/2);
                }

                else if (Comparar(funcion, "ir"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                        posXT = posix + Convert.ToInt32(tokens.ElementAt(i + 1).Lexema);
                    else
                        posXT = posix + Convert.ToInt32(obtenerValorVar(tokens.ElementAt(i + 1).Lexema));

                    if (tokens.ElementAt(i + 3).IdToken == 13)
                        posYT = posiy + Convert.ToInt32(tokens.ElementAt(i + 3).Lexema);
                    else
                        posYT = posiy + Convert.ToInt32(obtenerValorVar(tokens.ElementAt(i + 3).Lexema));

                    tortuga.Location = new Point(posXT-tortuga.Width/2, posYT-tortuga.Height/2);
                    i = i + 4;
                }

                else if (Comparar(funcion, "irx") || Comparar(funcion, "ix"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                        posXT = posix + Convert.ToInt32(tokens.ElementAt(i + 1).Lexema);
                    else
                        posXT = posix + Convert.ToInt32(obtenerValorVar(tokens.ElementAt(i + 1).Lexema));

                    tortuga.Location = new Point(posXT - tortuga.Width / 2, posYT - tortuga.Height / 2);
                    i = i + 2;
                }

                else if (Comparar(funcion, "iry") || Comparar(funcion, "iy"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                        posYT = posiy + Convert.ToInt32(tokens.ElementAt(i + 1).Lexema);
                    else
                        posYT = posiy + Convert.ToInt32(obtenerValorVar(tokens.ElementAt(i + 1).Lexema));

                    tortuga.Location = new Point(posXT - tortuga.Width / 2, posYT - tortuga.Height / 2);
                    i = i + 2;
                }

                else if (Comparar(funcion, "subirpincel") || Comparar(funcion,"spl"))
                    bajarLapiz = false;

                else if (Comparar(funcion, "bajarpincel") || Comparar(funcion,"bpl"))
                    bajarLapiz = true;

                else if(Comparar(funcion,"avanzar") || Comparar(funcion, "avz"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                        moverTortuga(Convert.ToInt32(tokens.ElementAt(i + 1).Lexema),-1);
                    else
                        moverTortuga(Convert.ToInt32(obtenerValorVar(tokens.ElementAt(i + 1).Lexema)), -1);

                    i = i + 2;
                }

                else if(Comparar(funcion,"retroceder") || Comparar(funcion, "ret"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                        moverTortuga(Convert.ToInt32(tokens.ElementAt(i + 1).Lexema), +1);
                    else
                        moverTortuga(Convert.ToInt32(obtenerValorVar(tokens.ElementAt(i + 1).Lexema)), +1);

                    i = i + 2;
                }

                else if(Comparar(funcion, "girarIzq") || Comparar(funcion, "izq"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                    {
                        angulo -= Convert.ToDouble(tokens.ElementAt(i + 1).Lexema);
                        anguloT -= Convert.ToDouble(tokens.ElementAt(i + 1).Lexema);
                        tortuga.Image = rotarImagen(Imagen, anguloT);
                    }
                    else
                    {
                        angulo -= Convert.ToDouble(obtenerValorVar(tokens.ElementAt(i + 1).Lexema));
                        anguloT -= Convert.ToDouble(obtenerValorVar(tokens.ElementAt(i + 1).Lexema));
                        tortuga.Image = rotarImagen(Imagen, anguloT);
                    }
                    i = i + 2;
                }

                else if (Comparar(funcion, "girarDer") || Comparar(funcion, "der"))
                {
                    if (tokens.ElementAt(i + 1).IdToken == 13)
                    {
                        angulo += Convert.ToDouble(tokens.ElementAt(i + 1).Lexema);
                        anguloT += Convert.ToDouble(tokens.ElementAt(i + 1).Lexema);
                        tortuga.Image = rotarImagen(Imagen, anguloT);
                    }
                    else
                    {
                        angulo += Convert.ToDouble(obtenerValorVar(tokens.ElementAt(i + 1).Lexema));
                        anguloT += Convert.ToDouble(obtenerValorVar(tokens.ElementAt(i + 1).Lexema));
                        tortuga.Image = rotarImagen(Imagen, anguloT);

                        i = i + 2;
                    }
                }
            }
            lienzo.Refresh();
        }

        private void tamañoLienzo(int x, int y)
        {
            int xlienzo = lienzo.Width;
            int ylienzo = lienzo.Height;
            g.DrawLine(pen, new Point(xlienzo / 2 - x / 2, ylienzo / 2 - y / 2),
                            new Point(xlienzo / 2 + x / 2, ylienzo / 2- y /2));
            g.DrawLine(pen, new Point(xlienzo / 2 - x / 2, ylienzo / 2 - y / 2),
                            new Point(xlienzo / 2 - x / 2, ylienzo / 2 + y / 2));
            g.DrawLine(pen, new Point(xlienzo / 2 + x / 2, ylienzo / 2 + y / 2),
                            new Point(xlienzo / 2 + x / 2, ylienzo / 2 - y / 2));
            g.DrawLine(pen, new Point(xlienzo / 2 + x / 2, ylienzo / 2 + y / 2),
                            new Point(xlienzo / 2 - x / 2, ylienzo / 2 + y / 2));
        }

        public void moverTortuga(int cantidad, int direccion)
        {
            int tx = posXT;
            int ty = posYT;

            posXT = posXT + Convert.ToInt32(direccion *cantidad * Math.Cos(angulo * Math.PI / 180));
            posYT = posYT + Convert.ToInt32(direccion * cantidad * Math.Sin(angulo * Math.PI / 180));

            if (bajarLapiz)
                g.DrawLine(pen, new Point(tx,ty),new Point(posXT,posYT));

            tortuga.Location = new Point(posXT - (tortuga.Width / 2), posYT - (tortuga.Height / 2));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lienzo.BackColor = Color.White;
            tortuga.Parent = lienzo;
            tortuga.BackColor = Color.Transparent;
        }

        public Double obtenerValorVar(String nombre)
        {
            Boolean variableEncontrada = false;
            int pos = 0;

            for (int i = 0; i < variables.Count && !variableEncontrada; i++)
                if (variables.ElementAt(i).Id == nombre)
                {
                    variableEncontrada = true;
                    pos = i;
                }

            if (variableEncontrada)
                return Convert.ToDouble(variables.ElementAt(pos).Valor);
            else
                return 0;
        }

        public Boolean Comparar(String cadena1, String cadena2)
        {
            if (String.Compare(cadena1, cadena2, true) == 0)
                return true;
            else
                return false;
        }

        public Color obtenerColorLienzo(String color)
        {
            if (Comparar(color, "blanco"))
                return Color.White;
            else if (Comparar(color, "celeste"))
                return Color.SkyBlue;
            else if (Comparar(color, "amarillo"))
                return Color.Yellow;
            else
                return Color.White;
        }

        public Color obtenerColorPincel(String color)
        {
            if (Comparar(color, "rojo"))
                return Color.Red;
            else if (Comparar(color, "azul"))
                return Color.Blue;
            else if (Comparar(color, "negro"))
                return Color.Black;
            else
                return Color.Black;
        }

        public void restablecerHerramientas()
        {
            //CREAR ENTORNO PARA DIBUJAR
            b = new Bitmap(lienzo.Width, lienzo.Height);
            lienzo.Image = (Image)b;
            g = Graphics.FromImage(b);

            //CREANDO LAPIZ
            pen = new Pen(Color.Black);
            bajarLapiz = false;
            angulo = 90;
            anguloT = 0;

            //COORDENADAS TORTUGA
            posXT = lienzo.Width / 2;
            posYT = lienzo.Height / 2;
            tortuga.Location = new Point(posXT-tortuga.Width/2, posYT-tortuga.Height/2);
            Imagen = new Bitmap("tortuga2.png", true);
            tortuga.Image = Imagen;

            //ESTABLECIENDO CONCORDANCIA ENTRE COORDENADAS TORTUGA CON EL LIENZO            
            tamañox = lienzo.Width;
            tamañoy = lienzo.Height;
            posix = Convert.ToInt32(lienzo.Width / 2 - tamañox / 2);
            posiy = Convert.ToInt32(lienzo.Height / 2 - tamañoy / 2);
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lenguaje Formales y de Programacción- A \n"+
                            "Ruth Nohemy Ardón Lechuga \n" +
                            "Carnet: 201602975");
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog cuadroDialogo = new SaveFileDialog();
            cuadroDialogo.Title = "Seleccionar ubicación para guardar archivo";
            cuadroDialogo.Filter = "Documentos de texto(*.ktl) | *.ktl";

            if (cuadroDialogo.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter escritor = new StreamWriter(cuadroDialogo.FileName);
                    ruta = cuadroDialogo.FileName;
                    escritor.Write(editor.Text);
                    escritor.Close();

                    MessageBox.Show("Archivo guardado Satisfactoriamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al intentar guardar el archivo" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error al intentar encontrar la dirección");
            }

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog cuadroDialogo = new OpenFileDialog();
            cuadroDialogo.Title = "Seleccionar archivo";
            cuadroDialogo.Filter = "Documentos de texto (*.ktl)|*.ktl";

            if ((cuadroDialogo.ShowDialog()) == DialogResult.OK)
            {
                try
                {
                    StreamReader lector = new StreamReader(cuadroDialogo.FileName);
                    ruta = cuadroDialogo.FileName;
                    String texto = lector.ReadToEnd();
                    editor.Text = texto;
                    lector.Close();

                    MessageBox.Show("Archivo abierto Satisfactoriamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("¡Error! Ha habido un problema al intentar abrir el archivo" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error al intentar encontrar la dirección");
            }
        }

        private Bitmap rotarImagen(Bitmap b, Double ang)
        {
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            Graphics g = Graphics.FromImage(returnBitmap);

            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            g.RotateTransform((float)ang);
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            g.DrawImage(b, new Point(0,0));

            return returnBitmap;
        }

        private void reporteDeTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String HTML =
            "<html><head><style> table {width: 60%; border: 1px solid black;}\n" +
            "th, td {text-align: cennter;padding: 8px; border: 1px solid black;}\n" +
            "tr:nth-child(even){background-color: white}\n" +
            "th {background-color: green;color: white; }\n"+
            "</style></head><body><h1>TABLA TOKENS</h1><br><br>"
            + "<center><table>"
            + "<tr><th>#</th><th>Lexema</th><th>Fila</th><th>Columna</th>"
            + "<th>Id Token</th><th>Token</th>";

            for (int i = 0; i < tokens.Count; i++)
                HTML += "<tr>" +
                    "<td>" + (i+1).ToString() + "</td>" +
                    "<td>" + tokens.ElementAt(i).Lexema + "</td>" +
                    "<td>" + tokens.ElementAt(i).Fila.ToString() + "</td>" +
                    "<td>" + tokens.ElementAt(i).Columna.ToString() + "</td>" +
                    "<td>" + tokens.ElementAt(i).IdToken.ToString() + "</td>" +
                    "<td>" + tokens.ElementAt(i).Tipo.ToString()+ "</td>";

            HTML += "</Table></center></body></html>";

            StreamWriter escritor = new StreamWriter("TablaTokens.html");
            escritor.Write(HTML);
            escritor.Close();

            Process.Start("TablaTokens.html");
        }

        private void reporteDeErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String HTML =
                        "<html><head><style> table {width: 60%; border: 1px solid black;}\n" +
                        "th, td {text-align: cennter;padding: 8px; border: 1px solid black;}\n" +
                        "tr:nth-child(even){background-color: white}\n" +
                        "th {background-color: green;color: white; }\n" +
                        "</style></head><body><h1>TABLA ERRORES</h1><br><br>"
                        + "<center><table>"
                        + "<tr><th>#</th><th>Fila</th><th>Columna</th>"
                        + "<th>Caracter</th><th>Descripcion</th>";

            for (int i = 0; i < errores.Count; i++)
                HTML += "<tr>" +
                    "<td>" + (i+1).ToString() + "</td>" +
                    "<td>" + errores.ElementAt(i).Fila.ToString() + "</td>" +
                    "<td>" + errores.ElementAt(i).Columna.ToString() + "</td>" +
                    "<td>" + errores.ElementAt(i).Palabra + "</td>" +
                    "<td>" + errores.ElementAt(i).Descripcion + "</td>";

            HTML += "</Table></center></body></html>";

            StreamWriter escritor = new StreamWriter("TablaErrores.html");
            escritor.Write(HTML);
            escritor.Close();

            Process.Start("TablaErrores.html");
        }

        private void manualDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Manual de usuario.pdf");
        }

        private void manualDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Manual técnico.pdf");
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ruta != "")
            {
                StreamWriter escritor = new StreamWriter(ruta);
                escritor.Write(editor.Text);
                escritor.Close();
                MessageBox.Show("Archivo guardado Satisfactoriamente");
            } else
            {
                MessageBox.Show("El archivo no se ha guardado con anterioridad");
            }
        }
    }
}
