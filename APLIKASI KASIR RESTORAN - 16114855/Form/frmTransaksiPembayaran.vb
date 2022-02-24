Public Class frmTransaksiPembayaran 
    Dim Tampung As New DataTable
    Public PilihPemesanan As Boolean

    Sub TampilTabel()
        Tampung = ExecuteQuery("select a.ID_Pemesanan, b.ID_Pelanggan, d.Nama_Pelanggan, c.ID_Menu, c.Nama_Menu, a.Harga, a.Jumlah, 0.00 as TotalPembayaran from pemesanan_detil a, pemesanan b, menu_makanan c, pelanggan d where a.ID_Pemesanan=b.ID_Pemesanan and b.ID_Pemesanan='000000000000'")
        GridControl1.DataSource = Tampung
        ExecuteGridViewAllAppearance(GridView1, , "ID_Menu,ID_Pelanggan", "Harga,Jumlah,TotalPembayaran", , "TotalPembayaran", , "TotalPembayaran")

    End Sub

    Sub TampilInfoPembayaran()
        If GridView1.RowCount > 0 Then
            Dim TotalBayar As Double = GridView1.Columns("TotalPembayaran").SummaryItem.SummaryValue
            Dim TotalPembayaran As Double = TotalBayar
            txtBayar.Value = TotalBayar
            txtTotalPembayaran.Text = "Rp. " & Format(TotalPembayaran, "#,##0") & ",-"
        Else
            txtBayar.Value = 0
            txtTotalPembayaran.Text = "Rp. 0,-"
        End If
    End Sub

    Sub Databaru()
        TampilTabel()
        ExecuteInputTextValueClear(txtScan, txtIDPemesanan)
        txtBayar.Value = 0
        TampilInfoPembayaran()
        txtScan.Focus()

    End Sub

    Sub TampilListPemesanan()
        PilihPemesanan = False
        frmListPemesanan.NilaiKiriman = 1
        frmListPemesanan.ShowDialog()
        If PilihPemesanan = True Then
            TambahItem()
        End If
    End Sub

    Sub TambahItemDetil(ByVal ID_Pemesanan As String, ByVal ID_Pelanggan As String, ByVal Nama_Pelanggan As String, ByVal ID_Menu As String, ByVal Nama_Menu As String, ByVal Harga As Double, ByVal Jumlah As Integer, ByVal TotalPembayaran As Double)
        Dim CariDaftar = Tampung.Select("ID_Pemesanan='" & ID_Pemesanan & "'")
        If CariDaftar.Length <= 0 Then
            Dim BarisBaru = Tampung.NewRow
            BarisBaru.Item("ID_Pemesanan") = ID_Pemesanan
            BarisBaru.Item("ID_Pelanggan") = ID_Pelanggan
            BarisBaru.Item("Nama_Pelanggan") = Nama_Pelanggan
            BarisBaru.Item("ID_Menu") = ID_Menu
            BarisBaru.Item("Nama_Menu") = Nama_Menu
            BarisBaru.Item("Harga") = Harga
            BarisBaru.Item("Jumlah") = Jumlah
            BarisBaru.Item("TotalPembayaran") = (BarisBaru.Item("Harga") * BarisBaru.Item("Jumlah"))
            Tampung.Rows.Add(BarisBaru)
        Else
        End If
    End Sub

    Sub TambahItem()
        Dim CariIDPemesanan = ExecuteQuery("select a.ID_Pemesanan, b.ID_Pelanggan, d.Nama_Pelanggan, c.ID_Menu, c.Nama_Menu, a.Harga, a.Jumlah, 0.00 as TotalPembayaran from (pemesanan_detil a inner join pemesanan b on a.ID_Pemesanan=b.ID_Pemesanan inner join pelanggan d on b.ID_Pelanggan=d.ID_Pelanggan inner join menu_makanan c on a.ID_Menu=c.ID_Menu and b.ID_Pemesanan='" & txtScan.Text & "')").Select()
        If CariIDPemesanan.Length > 0 Then
            TambahItemDetil(CariIDPemesanan(0).Item("ID_Pemesanan"), CariIDPemesanan(0).Item("ID_Pelanggan"), CariIDPemesanan(0).Item("Nama_Pelanggan"), CariIDPemesanan(0).Item("ID_Menu"), CariIDPemesanan(0).Item("Nama_Menu"), CariIDPemesanan(0).Item("Harga"), CariIDPemesanan(0).Item("Jumlah"), CariIDPemesanan(0).Item("TotalPembayaran"))

                txtScan.Text = ""
                txtScan.Focus()
            Else
            MessageBox.Show("Pemesanan Tidak Ditemukan.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtScan.Text = ""
            txtScan.Focus()

            End If
        TampilInfoPembayaran()
    End Sub

    Private Sub frmTransaksiPembayaran_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtIDPemesanan.Properties.ReadOnly = True
        txtIDPemesanan.TabStop = False
        txtTanggal.DateTime = Now
        txtIDPengguna.Properties.ReadOnly = True
        txtIDPengguna.TabStop = False
        txtIDPengguna.Text = My.Settings.lgnIDPengguna
        txtBayar.Properties.ReadOnly = True
        txtBayar.TabStop = False
        txtInfoKasir.Text = "KASIR : " & My.Settings.lgnNamaPengguna
        Databaru()
    End Sub

    Private Sub txtScan_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtScan.ButtonClick
        TampilListPemesanan()

    End Sub

    Sub HapusItem()
        If GridView1.RowCount > 0 Then
            GridView1.DeleteSelectedRows()
        Else
            MessageBox.Show("Tidak ada daftar yang di pilih.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        TampilInfoPembayaran()
    End Sub


    Sub Simpan() 'menyimpan info pembelian dan pembayaran ke tabel pembelian,serta menyimpan daftar item pembelian ke tabel pembelian detil
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("Daftar Item pembayaran Masih Kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If txtIDPemesanan.Text = "" Or txtTanggal.Text = "" Or txtIDPengguna.Text = "" Then
            MessageBox.Show("ID Pemesanan, Tanggal, ID Pengguna Wajib Diisi!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim IDPemesanan As String = GridView1.GetFocusedRowCellValue("ID_Pemesanan")
        ExecuteQuery("UPDATE pemesanan SET ID_Pemesanan='" & IDPemesanan & "',Tanggal='" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "',ID_Pengguna='" & txtIDPengguna.Text & "',Status='DIBAYAR' WHERE ID_Pemesanan='" & txtIDPemesanan.Text & "'")
        'ExecuteQuery("INSERT INTO peminjaman(NomorPengembalian,Tanggal,NIP,KodeRuang,TanggalKembali,StatusPeminjaman,KeteranganKembali) VALUES ('" & txtNoPengembalian.Text & "','" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "','" & txtNIP.Text & "','" & txtKodeRuang.Text & "','" & Format(txtTanggalKembali.DateTime, "yyyy-MM-dd HH:mm:ss") & "', 'DIKEMBALIKAN','" & txtKeteranganKembali.Text & "')")

        For Each Isi In Tampung.Select()
            ExecuteQuery("INSERT INTO pembayaran(ID_Pemesanan,ID_Pengguna,Bayar) VALUES ('" & IDPemesanan & "','" & My.Settings.lgnIDPengguna & "', " & Isi.Item("TotalPembayaran") & ")")
        Next

        MessageBox.Show("Pembayaran Sukses!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        DataBaru()

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        HapusItem()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Simpan()


    End Sub

    Private Sub txtBayar_EditValueChanged(sender As Object, e As EventArgs) Handles txtBayar.EditValueChanged

    End Sub
End Class