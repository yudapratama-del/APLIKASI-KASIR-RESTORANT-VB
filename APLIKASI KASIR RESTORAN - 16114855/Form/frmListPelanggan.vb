Public Class frmListPelanggan 
    Dim Tampung As New DataTable
    Public NilaiKiriman As Integer

    Sub TampilTabel()
        Tampung = ExecuteQuery("select * from pelanggan")
        GridControl1.DataSource = Tampung
        ExecuteGridViewAllAppearance(GridView1)
    End Sub

    Sub Pilih()
        If GridView1.RowCount > 0 And GridView1.GetFocusedRowCellValue("ID_Pelanggan") <> "" Then
            Select Case NilaiKiriman
                Case 1
                    frmTransaksiPemesanan.txtIDPelanggan.Text = GridView1.GetFocusedRowCellValue("ID_Pelanggan")
                    frmTransaksiPemesanan.txtNamaPelanggan.Text = GridView1.GetFocusedRowCellValue("Nama_Pelanggan")
                Case (2)

            End Select
            Me.Close()
        Else
            MessageBox.Show("Belum ada data yang di pilih!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub frmListPelanggan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.Enter
                Pilih()
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub frmListPelanggan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        TampilTabel()
    End Sub

    Private Sub txtPencarian_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPencarian.EditValueChanged
        GridView1.FindFilterText = txtPencarian.Text
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Pilih()

    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Me.Close()

    End Sub
End Class