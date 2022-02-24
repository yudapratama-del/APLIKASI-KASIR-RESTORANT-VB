Public Class frmSetupLogin 
    Sub Login()
        Try
            'validasi inputan
            If txtUsername.Text = "" Or txtPassword.Text = "" Then
                MessageBox.Show("Username, dan Password wajib diisi!", "Validasi",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            'cek data pengguna dalam database sesuai inputan username di form
            Dim CariPengguna = ExecuteQuery("select * from pengguna where Username='" &
            txtUsername.Text & "'").Select()
            'cek data pencarian ditemukan / tidak
            If CariPengguna.Length > 0 Then
                'jika ditemukan cek password sama atau tidak dengan inputan password di form
                If CariPengguna(0).Item("Password") = txtPassword.Text Then
                    'cek yang login pengguna dengan kode PNG0000000 / tidak
                    If CariPengguna(0).Item("ID_Pengguna") = "PNG000000" Then
                        'jika yang login pengguna dengan kode PNG0000000
                        'isi data login ke my.settings
                        My.Settings.lgnIDPengguna = CariPengguna(0).Item("ID_Pengguna")
                        My.Settings.lgnNamaPengguna = CariPengguna(0).Item("NamaPengguna")
                        My.Settings.lgnIDPengguna = "ADMINISTRATOR"
                        'tampilkan menu utama
                        frmSetupMenuUtama.Show()
                        'tutup form login
                        Me.Close()
                    Else
                        'jika yang login bukan pengguna dengan kode PNG0000000
                        'cari hak akses pengguna 
                        Dim CariHakAkses = ExecuteQuery("select * from hak_akses where Nama_Akses='" & CariPengguna(0).Item("Nama_Akses") & "'").Select()
                        'jika hak akses ditemukan
                        If CariHakAkses.Length > 0 Then
                            'tampilkan menu utama sesuai hak akses yang ada
                            ExecuteCheckAccess(frmSetupMenuUtama.RibbonControl,
                            CariHakAkses(0).Item("Akses"))
                            'isi data login ke my.settings
                            My.Settings.lgnIDPengguna = CariPengguna(0).Item("ID_Pengguna")
                            My.Settings.lgnNamaPengguna = CariPengguna(0).Item("Nama_Pengguna")
                            My.Settings.lgnNamaAkses = CariPengguna(0).Item("Nama_Akses")
                            'tampilkan menu utama
                            frmSetupMenuUtama.Show()
                            'tutup form login
                            Me.Close()
                        Else
                            'jika hak akses tidak ditemukan
                            MessageBox.Show("Akses tidak ditemukan!", "Validasi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End If
                Else
                    'jika password salah
                    MessageBox.Show("Username/Password salah!", "Validasi", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                End If
            Else
                'jika username tidak ditemukan
                MessageBox.Show("Username/Password salah!", "Validasi", MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            'jika error koneksi maka akan diarahkan ke konfigurasi server
            If MessageBox.Show(ex.Message & vbCrLf & "Lanjutkan ke konfigurasi server?", "Validasi",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                frmSetupKonfigurasiServer.ShowDialog()
            End If
        End Try
    End Sub

    Private Sub fmSetupLogin_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                Login()
            Case Keys.Escape
                Application.Exit()
        End Select
    End Sub

    Private Sub fmSetupLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtUsername.Text = ""
        txtPassword.Text = ""
        txtPassword.Properties.UseSystemPasswordChar = True
        ExecuteSkin(My.Settings.SkinName)
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Login()
    End Sub


    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Application.Exit()
    End Sub
End Class