Imports MySql.Data.MySqlClient

Public Class frmSetupKonfigurasiServer
    Sub TesKoneksi()
        If txtServer.Text = "" Or txtUserID.Text = "" Or txtDatabase.Text = "" Then
            MessageBox.Show("Nama Server, Username, dan Nama Database harus diisi!", "Validasi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub 'Otomatis Keluar Sub
        End If

        Dim tempConString As String = My.Settings.ConString
        My.Settings.ConString = "server=" & txtServer.Text & ";user ID=" & txtUserID.Text & ";password=" & txtPassword.Text & ";database=" & txtDatabase.Text & ""
        Try
            ExecuteConnection(True)
            MessageBox.Show("Koneksi Berhasil", "Informasi",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Koneksi Gagal." & vbCrLf & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        My.Settings.ConString = tempConString
    End Sub

    Sub Simpan()
        If txtServer.Text = "" Or txtUserID.Text = "" Or txtDatabase.Text = "" Then
            MessageBox.Show("Nama Server, User ID, dan Nama Database harus diisi!", "Validasi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub 'Otomatis Keluar Sub
        End If

        Dim tempConString As String = My.Settings.ConString
        My.Settings.ConString = "server=" & txtServer.Text & ";user ID=" & txtUserID.Text & ";password=" & txtPassword.Text & ";database=" & txtDatabase.Text & ""
        Try
            ExecuteConnection(True)
            'menyimpan konfigurasi
            My.Settings.ConServer = txtServer.Text
            My.Settings.ConUserID = txtUserID.Text
            My.Settings.ConPassword = txtPassword.Text
            My.Settings.ConDatabase = txtDatabase.Text
            My.Settings.Save()
            Me.Close()
        Catch ex As Exception
            My.Settings.ConString = tempConString
            MessageBox.Show("Gagal Tersimpan." & vbCrLf & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Sub Tutup()
        Me.Close()
    End Sub


    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Simpan()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Tutup()
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        TesKoneksi()

    End Sub
End Class