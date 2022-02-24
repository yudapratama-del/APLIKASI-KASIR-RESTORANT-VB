Public Class frmSetupPengguna 
    Dim Tampung As New DataTable
    Dim StatusDataBaru As Boolean

    Sub TampilTabel()
        Tampung = ExecuteQuery("select ID_Pengguna,Nama_Pengguna,Jabatan,Alamat,Nomor_Telepon,Nama_Akses,Username from pengguna")
        GridControl1.DataSource = Tampung
        ExecuteGridViewAllAppearance(GridView1)
    End Sub

    Sub DataBaru()
        StatusDataBaru = True
        ExecuteInputTextValueClear(txtNamaPengguna, txtJabatan, txtAlamat, txtNoTelp, txtHakAkses, txtUsername, txtPassword)
        txtIDPengguna.Text = ExecuteAutoCode("pengguna", "ID_Pengguna", "PNG", "000000")
        TampilTabel()
        txtNamaPengguna.Focus()
    End Sub

    Sub Simpan()
        If txtIDPengguna.Text = "" Or txtNamaPengguna.Text = "" Or txtJabatan.Text = "" Or txtHakAkses.Text = "" Or txtUsername.Text = "" Or txtPassword.Text = "" Then
            MessageBox.Show("ID Pengguna, Nama Pengguna, Jabatan, Nama Akses, Username, dan Password Harus Diisi!!", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If StatusDataBaru = True Then
            Dim Cari = Tampung.Select("ID_Pengguna='" & txtIDPengguna.Text & "'")
            If Cari.Length <= 0 Then
                'INSERT
                ExecuteQuery("INSERT INTO pengguna(ID_Pengguna,Nama_Pengguna,Jabatan,Alamat,Nomor_Telepon,Nama_Akses,Username,Password) VALUES ('" & txtIDPengguna.Text & "','" & txtNamaPengguna.Text & "','" & txtJabatan.Text & "','" & txtAlamat.Text & "','" & txtNoTelp.Text & "','" & txtHakAkses.Text & "','" & txtUsername.Text & "','" & txtPassword.Text & "')")
                DataBaru()
                MessageBox.Show("DataBerhasil Disimpan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Data Gagal Disimpan." & vbCrLf & "Data sudah ada dalam database.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            'UPDATE
            ExecuteQuery("UPDATE pengguna SET Nama_Pengguna='" & txtNamaPengguna.Text & "',Jabatan='" & txtJabatan.Text & "',Alamat='" & txtAlamat.Text & "',Nomor_Telepon='" & txtNoTelp.Text & "',Nama_Akses='" & txtHakAkses.Text & "',Username='" & txtUsername.Text & "',Password='" & txtPassword.Text & "' WHERE ID_Pengguna='" & txtIDPengguna.Text & "' ")
            DataBaru()
            MessageBox.Show("DataBerhasil Disimpan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If
    End Sub

    Sub Ubah()
        If GridView1.RowCount > 0 Then
            StatusDataBaru = False
            txtIDPengguna.Text = GridView1.GetFocusedRowCellValue("ID_Pengguna")
            txtNamaPengguna.Text = GridView1.GetFocusedRowCellValue("Nama_Pengguna")
            txtJabatan.Text = GridView1.GetFocusedRowCellValue("Jabatan")
            txtAlamat.Text = GridView1.GetFocusedRowCellValue("Alamat")
            txtNoTelp.Text = GridView1.GetFocusedRowCellValue("Nomor_Telepon")
            txtHakAkses.Text = GridView1.GetFocusedRowCellValue("Nama_Akses")
            txtUsername.Text = GridView1.GetFocusedRowCellValue("Username")
            txtPassword.Text = ExecuteQuery("select * from pengguna where ID_Pengguna='" & GridView1.GetFocusedRowCellValue("ID_Pengguna") & "'").Select()(0).Item("Password")
            txtNamaPengguna.Focus()
        Else
            MessageBox.Show("Tidak ada data yang dipilih", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Sub Hapus()
        If GridView1.RowCount > 0 Then
            If MessageBox.Show("Data akan dihapus. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                'DELETE
                ExecuteQuery("DELETE FROM pengguna WHERE ID_Pengguna='" & GridView1.GetFocusedRowCellValue("ID_Pengguna") & "'")
                DataBaru()
            End If
        Else
            MessageBox.Show("Tidak ada data yang dipilih!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Sub Cetak()
        GridControl1.ShowRibbonPrintPreview()
    End Sub

    Private Sub frmSetupPengguna_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case e.Control And Keys.Enter
                Simpan()
            Case Keys.F5
                DataBaru()
            Case Keys.Escape
                Me.Close()
            Case Keys.F2
                Ubah()
            Case e.Control And Keys.Delete
                Hapus()
            Case Keys.F8
                Cetak()
        End Select
    End Sub

    Private Sub txtHakAkses_GotFocus(sender As Object, e As EventArgs) Handles txtHakAkses.GotFocus
        ExecuteComboBoxList("hak_akses", "Nama_Akses", txtHakAkses)
    End Sub

    Private Sub frmSetupPengguna_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtIDPengguna.ReadOnly = True
        txtIDPengguna.TabStop = False
        txtHakAkses.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        txtPassword.Properties.UseSystemPasswordChar = True
        DataBaru()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Simpan()

    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        Me.Close()

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        DataBaru()

    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Ubah()

    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Hapus()

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Cetak()

    End Sub
End Class