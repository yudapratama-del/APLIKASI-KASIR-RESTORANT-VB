Public Class frmTransaksiPemesanan 
    Dim Tampung As New DataTable
    Public PilihMenu As Boolean

    Sub TampilTabel()
        Tampung = ExecuteQuery("select a.ID_Menu,b.Nama_Menu,b.Kategori,a.Harga,a.Jumlah,0.00 as TotalPemesanan from Pemesanan_detil a,menu_makanan b where a.ID_Menu=b.ID_Menu and a.ID_Pemesanan='000000000000'")
        GridControl1.DataSource = Tampung
        ExecuteGridViewAllAppearance(GridView1, , , "Harga,Jumlah,TotalPemesanan", , "TotalPemesanan", , "TotalPemesanan", , "ID_Menu,Nama_Menu,Kategori,TotalPemesanan")
    End Sub

    Sub TampilInfoPembayaran()
        If GridView1.RowCount > 0 Then
            Dim TotalPemesanan As Double = GridView1.Columns("TotalPemesanan").SummaryItem.SummaryValue
            Dim TotalPembayaran As Double = TotalPemesanan
            txtTotalPemesanan.Value = TotalPemesanan
            txtTotalPembayaran.Text = "Rp. " & Format(TotalPembayaran, "#,##0") & ",-"
        Else
            txtTotalPemesanan.Value = 0
            txtTotalPembayaran.Text = "Rp. 0,-"
        End If

    End Sub

    Sub Databaru()
        TampilTabel()
        ExecuteInputTextValueClear(txtScan, txtIDPemesanan, txtIDPelanggan, txtNamaPelanggan, txtDeskripsi)
        txtTanggal.DateTime = Now
        txtTotalPemesanan.Value = 0
        txtIDPemesanan.Text = ExecuteAutoCode("pemesanan", "ID_Pemesanan", "PSN", "000000000000")
        TampilInfoPembayaran()
        txtScan.Focus()

    End Sub

    Sub TampilListMenu()
        PilihMenu = False
        frmListMenu.NilaiKiriman = 1
        frmListMenu.ShowDialog()
        If PilihMenu = True Then
            TambahItem()
        End If
    End Sub

    Sub TampilListPelanggan()
        frmListPelanggan.NilaiKiriman = 1
        frmListPelanggan.ShowDialog()
    End Sub

    Sub TambahItemDetil(ByVal ID_Menu As String, ByVal Nama_Menu As String, ByVal Kategori As String, ByVal Harga As Double)
        Dim CariDaftar = Tampung.Select("ID_Menu='" & ID_Menu & "'")
        If CariDaftar.Length <= 0 Then
            Dim BarisBaru = Tampung.NewRow
            BarisBaru.Item("ID_Menu") = ID_Menu
            BarisBaru.Item("Nama_Menu") = Nama_Menu
            BarisBaru.Item("Kategori") = Kategori
            BarisBaru.Item("Harga") = Harga
            BarisBaru.Item("Jumlah") = 1
            BarisBaru.Item("TotalPemesanan") = (BarisBaru.Item("Harga") * BarisBaru.Item("Jumlah"))
            Tampung.Rows.Add(BarisBaru)
        Else
            CariDaftar(0).Item("Jumlah") = CariDaftar(0).Item("Jumlah") + 1
            CariDaftar(0).Item("TotalPemesanan") = (CariDaftar(0).Item("Harga") * CariDaftar(0).Item("Jumlah"))

        End If

    End Sub

    Sub TambahItem()
        Dim CariIdMenu = ExecuteQuery("select * from menu_makanan where ID_Menu='" & txtScan.Text & "'").Select()
        If CariIdMenu.Length > 0 Then
            TambahItemDetil(CariIdMenu(0).Item("ID_Menu"), CariIdMenu(0).Item("Nama_Menu"), CariIdMenu(0).Item("Kategori"), CariIdMenu(0).Item("Harga"))
            txtScan.Text = ""
            txtScan.Focus()
        Else
            Dim CariKodeBarang = ExecuteQuery("select * from menu_makanan where ID_Menu='" & txtScan.Text & "'").Select()
            If CariKodeBarang.Length > 0 Then
                TambahItemDetil(CariKodeBarang(0).Item("ID_Menu"), CariKodeBarang(0).Item("Nama_Menu"), CariKodeBarang(0).Item("Kategori"), CariKodeBarang(0).Item("Harga"))
                txtScan.Text = ""
                txtScan.Focus()
            Else
                MessageBox.Show("Menu Tidak Ditemukan.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtScan.Text = ""
                txtScan.Focus()
            End If
        End If
        TampilInfoPembayaran()
    End Sub

    Sub HapusItem()
        If GridView1.RowCount > 0 Then
            GridView1.DeleteSelectedRows()
        Else
            MessageBox.Show("Tidak ada daftar yang di pilih.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        TampilInfoPembayaran()
    End Sub

    Sub Simpan()
        If GridView1.RowCount <= 0 Then
            MessageBox.Show("Daftar Item Pemesanan Masih Kosong.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If txtIDPemesanan.Text = "" Or txtTanggal.Text = "" Or txtIDPelanggan.Text = "" Or txtNamaPelanggan.Text = "" Then
            MessageBox.Show("ID Pemesanan, Tanggal, ID Pelanggan dan Nama Pelanggan Wajib Diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If txtTotalPemesanan.Value <= 0 Then
            MessageBox.Show("Total Pemesanan tidak valid.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim ID_Pemesanan As String = ExecuteAutoCode("pemesanan", "ID_Pemesanan", "PSN", "000000000000")

        ExecuteQuery("INSERT INTO pemesanan(ID_Pemesanan,Tanggal,ID_Pelanggan,Deskripsi,ID_Pengguna,Status) VALUES ('" & ID_Pemesanan & "','" & Format(txtTanggal.DateTime, "yyyy-MM-dd HH:mm:ss") & "','" & txtIDPelanggan.Text & "','" & txtDeskripsi.Text & "','" & My.Settings.lgnIDPengguna & "','DIPESAN')")
        For Each Isi In Tampung.Select()
            ExecuteQuery("INSERT INTO pemesanan_detil(ID_Pemesanan,ID_Menu,Harga,Jumlah) VALUES ('" & ID_Pemesanan & "','" & Isi.Item("ID_Menu") & "'," & Isi.Item("Harga") & "," & Isi.Item("Jumlah") & ")")
        Next

        Databaru()
        MessageBox.Show("Pemesanan Sukses!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Sub Tutup()
        If GridView1.RowCount > 0 Then
            If MessageBox.Show("Daftar Pesanan Sudah Terisi. Lanjutkan tutup Transaksi?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub


    Private Sub frmTransaksiPemesanan_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F1
                TampilListMenu()
            Case Keys.F3
                TampilListPelanggan()
            Case e.Control And Keys.Delete
                HapusItem()
            Case e.Control And Keys.Enter
                Simpan()
            Case Keys.F5
                Databaru()
            Case Keys.Escape
                Tutup()
        End Select
    End Sub

    Private Sub frmTransaksiPemesanan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtIDPemesanan.Properties.ReadOnly = True
        txtIDPemesanan.TabStop = False
        txtIDPelanggan.Properties.ReadOnly = True
        txtIDPelanggan.TabStop = False
        txtNamaPelanggan.Properties.ReadOnly = True
        txtNamaPelanggan.TabStop = False
        txtTotalPemesanan.Properties.ReadOnly = True
        txtTotalPemesanan.TabStop = False
        txtInfoKasir.Text = "WAITER : " & My.Settings.lgnNamaPengguna
        Databaru()
    End Sub

    Private Sub txtScan_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtScan.ButtonClick
        TampilListMenu()

    End Sub

    Private Sub txtScan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtScan.KeyPress
        If e.KeyChar = Chr(13) Then
            TambahItem()
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        TambahItem()

    End Sub

    Private Sub txtIDPelanggan_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtIDPelanggan.ButtonClick
        TampilListPelanggan()

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        HapusItem()

    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Simpan()

    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        Databaru()

    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()

    End Sub

    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        Select Case e.Column.GetTextCaption
            Case "Harga"
                GridView1.SetFocusedRowCellValue("TotalPemesanan", (GridView1.GetFocusedRowCellValue("Harga") * GridView1.GetFocusedRowCellValue("Jumlah")))
            Case "Jumlah"
                GridView1.SetFocusedRowCellValue("TotalPemesanan", (GridView1.GetFocusedRowCellValue("Harga") * GridView1.GetFocusedRowCellValue("Jumlah")))
        End Select
        GridView1.RefreshData()
        GridView1.ExpandAllGroups()
        GridView1.BestFitColumns()
        TampilInfoPembayaran()
    End Sub

    Private Sub txtInfoKasir_Click(sender As Object, e As EventArgs) Handles txtInfoKasir.Click

    End Sub
End Class