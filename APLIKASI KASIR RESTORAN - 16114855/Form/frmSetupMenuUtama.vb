Public Class frmSetupMenuUtama 
    Private Sub frmSetupMenuUtama_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'memastikan aplikasi tertutup jika form login tidak ditampilkan
        If frmSetupLogin.Visible = False Then
            Application.Exit()
        End If
    End Sub

    Private Sub frmSetupMenuUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True

        ExecuteWalpaper(Me, My.Settings.ImageLocation)
        ExecuteSkin(My.Settings.SkinName, RibbonGalleryBarItem1)
        BarButtonItem3.Caption = "Pengguna : " & My.Settings.lgnNamaPengguna
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        BarButtonItem4.Caption = "Jam : " & Format(Now, "HH:mm:ss") & " Hari : " & Format(Now, "dddd") & " Tanggal : " & Format(Now, "dd MMMM yyyy")
    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        'logout
        If MessageBox.Show("Akses Pengguna Akan Ditutup !" & vbCrLf & "Lanjutkan?", "Verifikasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            With frmSetupLogin
                .Show()
                Me.Close()
            End With
        End If
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        'keluar
        If frmSetupLogin.Visible <> True Then
            If MessageBox.Show("Aplikasi Akan Ditutup !" & vbCrLf & "Lanjutkan?", "Verifikasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
                Application.Exit()
            End If
        End If
    End Sub

    Private Sub RibbonGalleryBarItem1_GalleryItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs) Handles RibbonGalleryBarItem1.GalleryItemClick
        ExecuteSkinSave(e.Item.Caption)
    End Sub

    Private Sub BarButtonItem5_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        ExecuteWalpaperSave(Me)
    End Sub

    Private Sub BarButtonItem6_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        frmSetupKonfigurasiServer.ShowDialog()

    End Sub

    Private Sub BarButtonItem7_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        frmSetupHakAkses.ShowDialog()

    End Sub

    Private Sub BarButtonItem8_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem8.ItemClick
        frmSetupPengguna.MdiParent = Me
        frmSetupPengguna.Show()
    End Sub

    Private Sub BarButtonItem9_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem9.ItemClick
        frmReferensiMenu.MdiParent = Me
        frmReferensiMenu.Show()
    End Sub

    Private Sub BarButtonItem10_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem10.ItemClick
        frmReferensiPelanggan.MdiParent = Me
        frmReferensiPelanggan.Show()
    End Sub

    Private Sub BarButtonItem11_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem11.ItemClick
        frmTransaksiPemesanan.MdiParent = Me
        frmTransaksiPemesanan.Show()
    End Sub

    Private Sub BarButtonItem12_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem12.ItemClick
        frmTransaksiPembayaran.MdiParent = Me
        frmTransaksiPembayaran.Show()
    End Sub

    Private Sub BarButtonItem13_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        frmLaporanTransaksi.MdiParent = Me
        frmLaporanTransaksi.Show()
    End Sub

    Private Sub BarButtonItem13_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem13.ItemClick
        frmLaporanTransaksi.MdiParent = Me
        frmLaporanTransaksi.Show()

    End Sub
End Class