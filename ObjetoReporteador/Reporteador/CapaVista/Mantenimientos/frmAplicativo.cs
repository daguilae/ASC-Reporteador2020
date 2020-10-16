﻿using System;
using CapaControlador.ControladoresReporteador;
using System.Windows.Forms;
using CapaModelo.Clases_Reporteador;
using System.Text.RegularExpressions;

namespace CapaVista.Mantenimientos
{
    public partial class frmAplicativo : Form
    {
        private clsAplicativo aplicativo;
        private string sNombreAux, sDescAux;
        private int iIDAux, iIDModAux;
        clsControlAplicativo controlAplicativo=new clsControlAplicativo();
        public frmAplicativo()
        {
            InitializeComponent();
            CargarCombobox();
            cargarDatos();
            CargarBusqueda();
            BloquearBotones();
            ttMensaje.SetToolTip(this.txtDescripcion, "Ingrese la descripción del aplicativo");
            ttMensaje.SetToolTip(this.txtNombre, "Ingrese el nombre del aplicativo");
            ttMensaje.SetToolTip(this.cmbModulo, "Seleccione el módulo que corresponde al aplicativo");
            ttMensaje.SetToolTip(this.btnAyuda, "Accede a una ventana que explica el funcionamiento del formulario");
            ttMensaje.SetToolTip(this.btnGuardar, "Guarda los datos que ingresó");
            ttMensaje.SetToolTip(this.btnModificar, "Guarda los cambios de datos previamente seleccionados que usted modificó");
            ttMensaje.SetToolTip(this.btnRefrescar, "Actualiza las opciones de Datos a Buscar y Muestra todos los datos del Grid");
        }

        private void CargarCombobox()
        {
            cmbModulo.DisplayMember = "nombre_modulo";
            cmbModulo.ValueMember = "pk_id_modulo";
            cmbModulo.DataSource = controlAplicativo.obtenerCamposCombobox("pk_id_modulo","nombre_modulo","MODULO","estado_modulo");
            cmbModulo.SelectedIndex = -1;
        }
        private void CargarBusqueda()
        {
            cmbBuscar.DisplayMember = "nombre_aplicativo";
            cmbBuscar.ValueMember = "pk_id_aplicativo";
            cmbBuscar.DataSource = controlAplicativo.obtenerCamposCombobox("pk_id_aplicativo","nombre_aplicativo","APLICATIVO","estado_aplicativo");
            cmbBuscar.SelectedIndex = -1;
            cmbBuscar.Refresh();
        }
        private void cargarDatos()
        {
            dgvVistaDatos.DataSource = controlAplicativo.obtenerTodo();
        }
        private void BloquearBotones()
        {
            btnModificar.Enabled = false;
            btnGuardar.Enabled = true;
        }
        private clsAplicativo llenarCampos()
        {
            clsAplicativo auxAplicativo = new clsAplicativo();
            auxAplicativo.SNombre = txtNombre.Text;
            auxAplicativo.SDescripcion = txtDescripcion.Text;
            auxAplicativo.IModulo = int.Parse(cmbModulo.SelectedValue.ToString());
            auxAplicativo.IEstado = 1;
            return auxAplicativo;
        }

        private void LimpiarComponentes()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            cmbModulo.SelectedIndex = -1;
            txtNombre.Focus();
        }
        private clsAplicativo ObtenerModificaciones()
        {
            clsAplicativo auxAplicativo = new clsAplicativo();
            auxAplicativo.SNombre = txtNombre.Text;
            auxAplicativo.SDescripcion = txtDescripcion.Text;
            auxAplicativo.IModulo = int.Parse(cmbModulo.SelectedValue.ToString());
            auxAplicativo.IIdAplicativo = iIDAux;
            return auxAplicativo;
        }

        private bool guardarDatos()
        {
            this.aplicativo = llenarCampos();
            try
            {
                if (ValidarTextbox() == true)
                {
                    controlAplicativo.insertarAplicativo(this.aplicativo);
                    cargarDatos();
                    MessageBox.Show("Datos Correctamente Guardados", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar los Datos", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (guardarDatos() == true)
            {
                LimpiarComponentes();
            }
            else
            {
                LimpiarComponentes();
            }
        }
        private bool ModificarDatos()
        {
            this.aplicativo = ObtenerModificaciones();
            try
            {
                if (ValidarTextbox() == true)
                {
                    controlAplicativo.modificarAplicativo(this.aplicativo);
                    cargarDatos();
                    MessageBox.Show("Datos Correctamente Modificados", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Modificar los Datos", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return false;
                throw;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (ModificarDatos() == true)
            {
                LimpiarComponentes();
                BloquearBotones();
            }
            else
            {
                LimpiarComponentes();
                BloquearBotones();
            }
        }

        private void cmsEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dgMensaje = MessageBox.Show("Una vez eliminados estos datos no se podrán recuperar, ¿Desea Continuar?", "¡ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dgMensaje == DialogResult.Yes)
                {

                    this.controlAplicativo.eliminarAplicativo(iIDAux);
                    cargarDatos();
                    MessageBox.Show("Datos Correctamente Eliminados", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }else if (dgMensaje == DialogResult.No)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Eliminar los Datos", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
        }

        private void tmrHoraFecha_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToLongTimeString();
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void frmAplicativo_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult drResultadoMensaje;
            drResultadoMensaje = MessageBox.Show("¿Realmente desea salir?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (drResultadoMensaje == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnAyuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "AyudasReporteador/AyudasObjetoReporteador.chm", "Aplicativo.html");
        }

        private void cmbBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBuscar.SelectedIndex >= 0)
            {
                int iIDAux = int.Parse(cmbBuscar.SelectedValue.ToString());
                dgvVistaDatos.DataSource = controlAplicativo.obtenerDatos(iIDAux);
            }
            else if (cmbBuscar.SelectedIndex < 0)
            {
                cargarDatos();
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarBusqueda();
        }

        private void dgvVistaDatos_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                iIDAux = int.Parse(dgvVistaDatos.Rows[e.RowIndex].Cells["pk_id_aplicativo"].Value.ToString());
                sNombreAux = dgvVistaDatos.Rows[e.RowIndex].Cells["nombre_aplicativo"].Value.ToString();
                sDescAux = dgvVistaDatos.Rows[e.RowIndex].Cells["descripcion_aplicativo"].Value.ToString();
                iIDModAux=int.Parse(dgvVistaDatos.Rows[e.RowIndex].Cells["fk_id_modulo"].Value.ToString());
                this.cmsEM.Show(this.dgvVistaDatos, e.Location);
                cmsEM.Show(Cursor.Position);
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            char cCaracter = e.KeyChar;
            if (!char.IsLetter(cCaracter) && cCaracter != 8 && cCaracter != 32)
            {
                e.Handled = true;
            }
        }

        private void txtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            char cCaracter = e.KeyChar;
            if (!char.IsLetterOrDigit(cCaracter) && cCaracter != 8 && cCaracter != 32)
            {
                e.Handled = true;
            }
        }

        private bool ValidarTextbox()
        {

            if (txtNombre.Text == "")
            {
                MessageBox.Show("Ingrese Nombre", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Text = "";
                txtNombre.Focus();
                return false;
            }
            else if (txtDescripcion.Text == "")
            {
                MessageBox.Show("Ingrese Descripción", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Text = "";
                txtNombre.Focus();
                return false;
            }
            else if (!Regex.Match(txtNombre.Text, @"^[A-Za-z]+([\ A-Za-z]+)*$").Success)
            {
                MessageBox.Show("Datos del campo nombre invalido", "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Text = "";
                txtNombre.Focus();
                return false;
            }
            if (txtNombre.Text == "" && txtDescripcion.Text == "")
            {
                MessageBox.Show("Llene los campos", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarComponentes();
                return false;
            }
            return true;

        }

        private void cmsModificar_Click(object sender, EventArgs e)
        {
            btnModificar.Enabled = true;
            btnGuardar.Enabled = false;
            txtNombre.Text = sNombreAux;
            txtDescripcion.Text = sDescAux;
        }
    }
}
