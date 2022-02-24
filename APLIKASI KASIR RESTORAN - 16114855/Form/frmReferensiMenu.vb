Public Class frmReferensiMenu 
    Dim tampung As DataTable
    Dim StatusDataBaru As Boolean

    Sub TampilTable()
        tampung = ExecuteQuery("select * from menu_makanan")
        GridControl1.DataSource = tampung
        ExecuteGridViewAllAppearance(GridView1, , , "Harga")
    End Sub

    Sub DataBaru()
        StatusDataBaru = True
        ExecuteInputTextValueClear(txtNamaMenu, txtKategori, txtDeskripsi)
        txtHarga.Value = 0
        txtID.Text = ExecuteAutoCode("menu_makanan", "ID_Menu", "MNU", "000000")
        TampilTable()
        txtNamaMenu.Focus()
    End Sub

    Sub Simpan()
        If txtID.Text = "" Or txtNamaMenu.Text = "" Or txtKategori.Text = "" Or txtHarga.Value = 0 Then
            MessageBox.Show("ID Menu,Nama Menu, Kategori dan Harga wajib diisi!", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If StatusDataBaru = True Then
            Dim Cari = tampung.Select("ID_Menu='" & txtID.Text & "'")
            If Cari.Length <= 0 Then
                'INSERT

                ExecuteQuery("INSERT INTO menu_makanan(ID_Menu,Nama_Menu,Kategori,Harga,Deskripsi) VALUES  ('" & txtID.Text & "','" & txtNamaMenu.Text & "','" & txtKategori.Text & "'," & txtHarga.Value & ",'" & txtDeskripsi.Text & "')")
                DataBaru()
                MessageBox.Show("Data berhasil disimpan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Data gagal disimpan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            'UPDATE
            ExecuteQuery("UPDATE menu_makanan SET Nama_Menu='" & txtNamaMenu.Text & "',Kategori='" & txtKategori.Text & "',Harga=" & txtHarga.Value & ",Deskripsi='" & txtDeskripsi.Text & "' WHERE ID_Menu='" & txtID.Text & "'")
            DataBaru()
            MessageBox.Show("Data berhasil disimpan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Sub Ubah()
        If GridView1.RowCount > 0 Then
            StatusDataBaru = False
            txtID.Text = GridView1.GetFocusedRowCellValue("ID_Menu")
            txtNamaMenu.Text = GridView1.GetFocusedRowCellValue("Nama_Menu")
            txtkategori.EditValue = GridView1.GetFocusedRowCellValue("Kategori")
            txtHarga.Value = GridView1.GetFocusedRowCellValue("Harga")
            txtdeskripsi.Text = GridView1.GetFocusedRowCellValue("Deskripsi")
            txtNamaMenu.Focus()
        Else
            MessageBox.Show("Tidak ada data yang dipilih!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Sub Hapus()
        If GridView1.RowCount > 0 Then

            If MessageBox.Show("Data akan dihapus. Lanjutkan?", "Validasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                'DELETE              
                ExecuteQuery("DELETE FROM menu_makanan WHERE ID_Menu='" & GridView1.GetFocusedRowCellValue("ID_Menu") & "'")
                DataBaru()
            End If
        Else
            MessageBox.Show("Tidak ada data yang dipilih!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Sub Cetak()
        GridControl1.ShowRibbonPrintPreview()
    End Sub

    Private Sub frmReferensiMenu_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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

    Private Sub frmReferensiMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        txtID.ReadOnly = True
        txtID.TabStop = False
        DataBaru()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Simpan()

    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
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


    Private Sub txtKategori_GotFocus(sender As Object, e As EventArgs) Handles txtKategori.GotFocus
        ExecuteComboBoxList("menu_makanan", "Kategori", txtKategori)
    End Sub
End Class