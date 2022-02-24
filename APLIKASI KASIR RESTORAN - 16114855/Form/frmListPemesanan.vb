Public Class frmListPemesanan 
    Dim Tampung As New DataTable
    Public NilaiKiriman As Integer

    Sub TampilTabel()
        Tampung = ExecuteQuery("select a.ID_Pemesanan,a.Status,b.Jumlah,b.Harga,d.ID_Pengguna,c.ID_Pelanggan from (pemesanan a inner join pemesanan_detil b on a.ID_Pemesanan=b.ID_Pemesanan inner join pelanggan c on a.ID_Pelanggan=c.ID_Pelanggan inner join pengguna d on a.ID_Pengguna=d.ID_Pengguna) where b.ID_Pemesanan=b.ID_Pemesanan and a.Status='DIPESAN'")
        GridControl1.DataSource = Tampung
        ExecuteGridViewAllAppearance(GridView1)
    End Sub

    Sub Pilih()
        If GridView1.RowCount > 0 And GridView1.GetFocusedRowCellValue("ID_Pemesanan") <> "" Then
            Select Case NilaiKiriman
                Case 1
                    frmTransaksiPembayaran.txtScan.Text = GridView1.GetFocusedRowCellValue("ID_Pemesanan")
                    frmTransaksiPembayaran.txtIDPemesanan.Text = GridView1.GetFocusedRowCellValue("ID_Pemesanan")
                    frmTransaksiPembayaran.PilihPemesanan = True
                Case 2

            End Select
            Me.Close()
        Else
            MessageBox.Show("Belum ada data yang di pilih!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub frmListPemesanan_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.Enter
                Pilih()
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub frmListPemesanan_Load(sender As Object, e As EventArgs) Handles Me.Load
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