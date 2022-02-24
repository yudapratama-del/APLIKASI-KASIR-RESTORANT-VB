Public Class frmDetilLaporanPemesanan 
    Dim Tampung As New DataTable
    Public ID_Pemesanan As String

    Sub TampilTabel()
        Select Case FrmLaporanTransaksi.txtKategori.SelectedIndex
            Case 0
                Tampung = ExecuteQuery("select a.ID_Pemesanan,b.ID_Menu,a.Harga,a.Jumlah,b.Nama_Menu from (pemesanan_detil a inner join menu_makanan b on a.ID_Menu=b.ID_Menu) where a.ID_Pemesanan='" & ID_Pemesanan & "'")
                Tampung.DefaultView.Sort = "ID_Pemesanan"
                GridControl1.DataSource = Tampung
                ExecuteGridViewAllAppearance(GridView1)
        End Select
    End Sub


    Sub Cetak()
        GridControl1.ShowRibbonPrintPreview()
    End Sub

    Sub Tutup()
        Me.Close()
    End Sub

    Private Sub FrmDetilLaporan_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.P
                Cetak()
            Case Keys.Escape
                Tutup()
        End Select
    End Sub

    Private Sub FrmDetilLaporan_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        Select Case FrmLaporanTransaksi.txtKategori.SelectedIndex
            Case 0
                Me.Text = "Detil Laporan Pemesanan No. Pemesanan " & ID_Pemesanan
        End Select


        TampilTabel()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Me.Close()
    End Sub
End Class