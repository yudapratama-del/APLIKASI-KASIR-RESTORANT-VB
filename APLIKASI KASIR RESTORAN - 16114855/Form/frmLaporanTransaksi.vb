Public Class frmLaporanTransaksi 
    Dim Tampung As New DataTable

    Sub TampilTabel()
        'membersihkan isian tabel
        GridControl1.DataSource = Nothing
        GridView1.Columns.Clear()
        Select Case txtKategori.SelectedIndex
            Case 0 'laporan pembelian harian
                Tampung = ExecuteQuery("select a.Tanggal,a.ID_Pemesanan,d.ID_Menu,a.ID_Pelanggan,a.ID_Pengguna,a.Status,b.Nama_Pengguna,c.Nama_Pelanggan from (pemesanan a inner join pengguna b on a.ID_Pengguna=b.ID_Pengguna inner join pelanggan c on a.ID_Pelanggan=c.ID_Pelanggan inner join pemesanan_detil d on a.ID_Pemesanan=d.ID_Pemesanan)where date_format(a.Tanggal,'%Y-%m-%d')between '" & Format(txtTanggalAwal.DateTime, "yyyy-MM-dd") & "' and '" & Format(txtTanggalAkhir.DateTime, "yyyy-MM-dd") & "' and Status='DIPESAN' and d.ID_Pemesanan=d.ID_Pemesanan")

                'For Each Isi In Tampung.Select()
                'Dim JumlahPinjam As Double = ExecuteQuery("select peminjaman_detil as JumlahPinjam where NomorPeminjaman='" & Isi.Item("NomorPeminjaman") & "'").Select()(0).Item("JumlahPinjam")
                ' Isi.Item("JumlahPinjam") = JumlahPinjam
                ' Next

                Tampung.DefaultView.Sort = "Tanggal,ID_Pemesanan" 'memfilter table
                GridControl1.DataSource = Tampung
                ExecuteGridViewAllAppearance(GridView1)

            Case 1
                Tampung = ExecuteQuery("select a.Tanggal,a.ID_Pemesanan,a.ID_Pelanggan,a.ID_Pengguna,a.Status,b.Nama_Pengguna,c.Nama_Pelanggan from (pemesanan a inner join pengguna b on a.ID_Pengguna=b.ID_Pengguna inner join pelanggan c on a.ID_Pelanggan=c.ID_Pelanggan inner join pembayaran d on a.ID_Pemesanan=d.ID_Pemesanan)where date_format(a.Tanggal,'%Y-%m-%d')between '" & Format(txtTanggalAwal.DateTime, "yyyy-MM-dd") & "' and '" & Format(txtTanggalAkhir.DateTime, "yyyy-MM-dd") & "' and Status='DIBAYAR' and d.ID_Pemesanan=d.ID_Pemesanan")

                ' For Each Isi In Tampung.Select()
                'Dim JumlahKembali As Double = ExecuteQuery("select pengembalian_detil as JumlahKembali where NomorPeminjaman='" & Isi.Item("NomorPeminjaman") & "'").Select()(0).Item("JumlahKembali")
                ' Isi.Item("JumlahKembali") = JumlahKembali
                ' Next

                Tampung.DefaultView.Sort = "Tanggal,ID_Pemesanan" 'memfilter table
                GridControl1.DataSource = Tampung
                ExecuteGridViewAllAppearance(GridView1)

        End Select
    End Sub

    Sub Cetak()
        Select Case txtKategori.SelectedIndex
            Case 0 'laporan per harian
                GridView1.OptionsPrint.RtfReportHeader = "LAPORAN PER PEMINJAMAN" & vbCrLf & "Periode " & Format(txtTanggalAwal.DateTime, "dd MMMM yyyy") & " s/d " & Format(txtTanggalAkhir.DateTime, "dd MMMM yyyy") & "" & vbCrLf
                GridView1.OptionsPrint.RtfReportFooter = vbCrLf & "Pimpinan" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "Krishna Adnyana"
                'ExecuteGridControlPreview(GridControl1, GridControl1.LookAndFeel, True, Printing.PaperKind.Folio, 10, 10, 10, 10)
            Case 1 'laporan per pemasok
                GridView1.OptionsPrint.RtfReportHeader = "LAPORAN PER PENGEMBALIAN" & vbCrLf & "Periode " & Format(txtTanggalAwal.DateTime, "dd MMMM yyyy") & " s/d " & Format(txtTanggalAkhir.DateTime, "dd MMMM yyyy") & "" & vbCrLf
                GridView1.OptionsPrint.RtfReportFooter = vbCrLf & "Pimpinan" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "Krishna Adnyana"
                '  ExecuteGridControlPreview(GridControl1, GridControl1.LookAndFeel, True, Printing.PaperKind.Folio, 10, 10, 10, 10)
        End Select
    End Sub
    Sub Tutup()
        Me.Close()
    End Sub

    Private Sub FrmLaporanTransaksi_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.Enter
                TampilTabel()
            Case e.Control And Keys.P
                Cetak()
            Case Keys.Escape
                Tutup()
            Case Keys.F1
                'DetilLaporan()
        End Select
    End Sub

    Private Sub FrmLaporanTransaksi_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        txtKategori.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        txtTanggalAwal.DateTime = Now
        txtTanggalAkhir.DateTime = Now
        txtKategori.SelectedIndex = 0
    End Sub
    Sub Detillaporan()
        Select Case txtKategori.SelectedIndex
            Case 0

                If GridView1.RowCount > 0 And GridView1.GetFocusedRowCellValue("ID_Pemesanan") <> "" Then
                    FrmDetilLaporanPemesanan.ID_Pemesanan = GridView1.GetFocusedRowCellValue("ID_Pemesanan")
                    FrmDetilLaporanPemesanan.StartPosition = FormStartPosition.CenterScreen
                    FrmDetilLaporanPemesanan.ShowDialog()
                Else
                    MessageBox.Show("Pilih Salah Satu Transaksi Terlebih Dahulu!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
                'Case 1
                '  If GridView1.RowCount > 0 And GridView1.GetFocusedRowCellValue("NomorPengembalian") <> "" Then
                'FrmDetilLaporan.NomorPengembalian = GridView1.GetFocusedRowCellValue("NomorPengembalian")
                ' FrmDetilLaporan.StartPosition = FormStartPosition.CenterScreen
                '  FrmDetilLaporan.ShowDialog()
                'Else
                'MessageBox.Show("Pilih Salah Satu Transaksi Terlebih Dahulu!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'End If

        End Select
    End Sub
    Private Sub txtKategori_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtKategori.SelectedIndexChanged
        TampilTabel()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        TampilTabel()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Cetak()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Tutup()
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Detillaporan()
    End Sub
End Class