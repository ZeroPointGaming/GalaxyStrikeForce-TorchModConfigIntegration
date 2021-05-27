Imports System.IO
Imports System.Environment

Public Class Form1
#Region "Public Declerations"
    Public Shortname As String = Nothing
    Public ProjectNamespaces As String = Nothing

    'Error Handling Stuff
    Public DefaultSettings As Boolean = False
    Public ColorSettings As Boolean = False
    Public HeatSettings As Boolean = False
    Public DefaultUISettings As Boolean = False
    Public CustomUISettings As Boolean = False
    Public BarrelSettings As Boolean = False
#End Region

#Region "Close Form Button"
    Private Sub MdButton6_Click(sender As Object, e As EventArgs) Handles MdButton6.Click
        Me.Close()
    End Sub
#End Region

#Region "Handle designer initializations"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Check if the save folder exists if not create it
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        If Not Directory.Exists(MyDocumentsFolder) Then
            Directory.CreateDirectory(MyDocumentsFolder)
        End If

        'Load saved templates from save folder
        For Each File In Directory.GetFiles(MyDocumentsFolder, "*.txt") 'Iterate saved templates into the listboxes
            Try
                Dim FilePath As String = File.ToString
                Dim FileName As String = FilePath.Replace(MyDocumentsFolder, "")

                'Sort templates into their respected listboxes
                If FileName.Contains("Default_Settings") Then
                    DefaultSettingsList.Items.Add(FileName)
                ElseIf FileName.Contains("Color_Settings") Then
                    ColorSettingsList.Items.Add(FileName)
                ElseIf FileName.Contains("Heat_Settings") Then
                    HeatSettingsList.Items.Add(FileName)
                ElseIf FileName.Contains("DefaultUI_Settings") Then
                    DefaultUISettingsList.Items.Add(FileName)
                ElseIf FileName.Contains("CustomUI_Settings") Then
                    CustomUISettingsList.Items.Add(FileName)
                ElseIf FileName.Contains("Barrel_Settings") Then
                    BarrelSettingsList.Items.Add(FileName)
                End If
            Catch ex As Exception
            End Try
        Next

        Dim MyDownloads As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Downloads")
        Dim MyDesktop As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Desktop")
        OutputLocationTextbox.Text = MyDesktop.ToString
    End Sub
#End Region

#Region "Default_Settings"
    'Default Settings | Load Template Function |> FINISHED <|
    Private Sub MdButton2_Click(sender As Object, e As EventArgs) Handles MdButton2.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToLoad As String = Nothing
        Dim Data() As String = Nothing

        Try 'Handles System.NullReferenceException if no item is selected in the listbox
            FileToLoad = DefaultSettingsList.SelectedItem.ToString
        Catch ex As Exception
            MessageBox.Show("No template file was selected to load!")
        End Try

        Dim FilePath As String = MyDocumentsFolder + FileToLoad

        Try 'Handles System.IO.FileNotFound Exception
            Data = My.Computer.FileSystem.ReadAllText(FilePath).Split("|")
        Catch ex As Exception
            Data = {Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing} 'Creates empty data array to prevent errors in data loading
        End Try

        If Not Data(0) Is Nothing Then AmmoClassNameVar.Text = Data(0)
        If Data(1) > 0 Then isSuperWeaponVar.Checked = True Else isSuperWeaponVar.Checked = False
        If Not Data(2) Is Nothing Then WeaponClassCountVar.Value = Data(2)
        If Not Data(3) Is Nothing Then WeaponRangeVar.Text = Data(3)
        If Not Data(4) Is Nothing Then PowerUsageVar.Text = Data(4)
        If Not Data(5) Is Nothing Then DamageVar.Text = Data(5)
        If Not Data(6) Is Nothing Then ShieldDamageVar.Text = Data(6)
        If Not Data(7) Is Nothing Then ExplosiveRadiusVar.Text = Data(7)
        If Not Data(8) Is Nothing Then BarrelCountVar.Value = Data(8)
    End Sub

    'Default Settings | Save Template Function | Needs Error Handling
    Private Sub MdButton1_Click(sender As Object, e As EventArgs) Handles ProjectNamespace.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"

        Dim FileToSave As String = "Default_Settings_" + DefaultSettingsTemplateNameTxtBox.Text + ".txt"
        Dim FilePath As String = MyDocumentsFolder + FileToSave
        Dim superweapon As String
        If isSuperWeaponVar.Checked = True Then superweapon = 1 Else superweapon = 0
        Dim Data As String = AmmoClassNameVar.Text + "|" + superweapon.ToString + "|" + WeaponClassCountVar.Value.ToString + "|" + WeaponRangeVar.Text + "|" + PowerUsageVar.Text + "|" + DamageVar.Text + "|" + ShieldDamageVar.Text + "|" + ExplosiveRadiusVar.Text + "|" + BarrelCountVar.Value.ToString

        My.Computer.FileSystem.WriteAllText(FilePath, Data, False)
        DefaultSettingsList.Items.Add(FileToSave)
    End Sub

    'Commit default settings to output box |> FINISHED <|
    Private Sub CommitDefaultSettingsBtn_Click(sender As Object, e As EventArgs) Handles CommitDefaultSettingsBtn.Click
        If DefaultSettings = True Then
            MessageBox.Show("Error: Default settings already committed!")
        Else
            'Project Settings
            If Not ProjectNamespaceTextbox.Text Is Nothing Then
                ProjectNamespaces = ProjectNamespaceTextbox.Text
            Else
                ProjectNamespaces = "NULL"
            End If
            If Not ShortNameTextbox.Text Is Nothing Then
                Shortname = ShortNameTextbox.Text
            Else
                Shortname = "NL"
            End If
            Dim UsingVrage As Boolean = False
            If UsingVrageCheckbox.Checked = True Then UsingVrage = True Else UsingVrage = False
            Dim UsingGSFConfig As Boolean = False
            If UsingGSFConfigCheckbox.Checked = True Then UsingGSFConfig = True Else UsingGSFConfig = False
            Dim UsingGSFWeapons As Boolean = False
            If UsingGSFWeaponsCheckbox.Checked = True Then UsingGSFWeapons = True Else UsingGSFWeapons = False
            If UsingVrage = True Then
                OutputBox.Text += "using VRageMath;" + NewLine
            End If
            If UsingGSFConfig = True Then
                OutputBox.Text += "using GSF.Config;" + NewLine
            End If
            If UsingGSFWeapons = True Then
                OutputBox.Text += "using GSF.GSFWeapons;" + NewLine
            End If
            OutputBox.Text += NewLine + "namespace " + ProjectNamespaces.ToString + NewLine + "{" + NewLine + "public static class " + Shortname + "_Default_Settings" + NewLine + "{" + NewLine + "public static BeamWeaponDefaultInfo " + ProjectNamespaceTextbox.Text + " = new BeamWeaponDefaultInfo(" + NewLine

            'Default Settings
            OutputBox.Text += Chr(34) + AmmoClassNameVar.Text.ToString + Chr(34) + "," + NewLine 'Ammo Class Name
            If isSuperWeaponVar.Checked = True Then OutputBox.Text += "true," + NewLine Else OutputBox.Text += "false," + NewLine 'Is Super Weapon
            OutputBox.Text += WeaponClassCountVar.Value.ToString + "," + NewLine 'Weapon Class Count (integer value)
            OutputBox.Text += WeaponRangeVar.Text.ToString + "," + NewLine 'Weapon Range (float value)
            OutputBox.Text += PowerUsageVar.Text.ToString + "," + NewLine 'Power Usage (float value)
            OutputBox.Text += DamageVar.Text.ToString + "," + NewLine 'Damage (float value)
            OutputBox.Text += ShieldDamageVar.Text.ToString + "," + NewLine 'Shield Damage (float value)
            OutputBox.Text += ExplosiveRadiusVar.Text.ToString + "," + NewLine 'Explosion Radius (float value)
            OutputBox.Text += BarrelCountVar.Value.ToString + NewLine + ");" + NewLine + "}" 'Barrel Count (integer value)

            DefaultSettings = True
        End If
    End Sub

    'Default Settings | Reset Button |> FINISHED <|
    Private Sub ForceValueResetDFSBtn_Click(sender As Object, e As EventArgs) Handles ForceValueResetDFSBtn.Click
        ProjectNamespaceTextbox.Text = "DefaultTemplate"
        ShortNameTextbox.Text = "DT"
        UsingVrageCheckbox.Checked = True
        UsingGSFConfigCheckbox.Checked = True
        UsingGSFWeaponsCheckbox.Checked = False
        isSuperWeaponVar.Checked = False
        AmmoClassNameVar.Text = "DefaultAmmoClass"
        WeaponClassCountVar.Value = 1
        WeaponRangeVar.Text = "200f"
        PowerUsageVar.Text = "250f"
        DamageVar.Text = "10f"
        ShieldDamageVar.Text = "10f"
        ExplosiveRadiusVar.Text = "0.05f"
        BarrelCountVar.Value = 1
    End Sub

    'Default Settings | Delete Template Button |> FINISHED <|
    Private Sub DefaultSettingsDeleteTemplateBtn_Click_1(sender As Object, e As EventArgs) Handles DefaultSettingsDeleteTemplateBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToDelete As String = Nothing
        Try
            FileToDelete = DefaultSettingsList.SelectedItem.ToString()
        Catch ex As Exception
            MessageBox.Show("No template file selected for deletion!")
        End Try

        If Not FileToDelete Is Nothing Then
            Dim FilePath As String = MyDocumentsFolder + FileToDelete
            My.Computer.FileSystem.DeleteFile(FilePath)
            DefaultSettingsList.Items.Remove(DefaultSettingsList.SelectedItem.ToString)
        End If
    End Sub
#End Region

#Region "Color Settings"
    'Color Settings | Reset Button
    Private Sub ColorSettingsResetBtn_Click(sender As Object, e As EventArgs) Handles ColorSettingsResetBtn.Click
        ExplosiveBeamAuxVar.Text = ""
        ExplosiveBeamAuxVar.BackColor = Color.FromArgb(20, 20, 20)
        ExplosiveBeamColorVar.Text = ""
        ExplosiveBeamColorVar.BackColor = Color.FromArgb(20, 20, 20)
        MainBeamWidthVar.Text = ""
        MainColorVar.Text = ""
        MainColorVar.BackColor = Color.FromArgb(20, 20, 20)
        AuxColorVar.Text = ""
        AuxColorVar.BackColor = Color.FromArgb(20, 20, 20)
        AuxBeamWidthVar.Text = ""
        ColorSettingsTemplateName.Text = "DefaultTemplate"
    End Sub

    'Color Selector | Explosive Beam Color
    Private Sub ExplosiveBeamColorVar_DoubleClick(sender As Object, e As EventArgs) Handles ExplosiveBeamColorVar.DoubleClick
        If ColorDialog1.ShowDialog(Me) Then
            Dim r As Integer = ColorDialog1.Color.R
            Dim b As Integer = ColorDialog1.Color.B
            Dim g As Integer = ColorDialog1.Color.G

            ExplosiveBeamColorVar.Text = "(" + (r.ToString + "," + b.ToString + "," + g.ToString + ")")
            ExplosiveBeamColorVar.BackColor = ColorDialog1.Color
        Else
        End If
    End Sub

    'Color Selector | Explosive Beam Aux
    Private Sub ExplosiveBeamAuxVar_DoubleClick(sender As Object, e As EventArgs) Handles ExplosiveBeamAuxVar.DoubleClick
        If ColorDialog1.ShowDialog(Me) Then
            Dim r As Integer = ColorDialog1.Color.R
            Dim b As Integer = ColorDialog1.Color.B
            Dim g As Integer = ColorDialog1.Color.G

            ExplosiveBeamAuxVar.Text = ("(" + r.ToString + "," + b.ToString + "," + g.ToString + ")")
            ExplosiveBeamAuxVar.BackColor = ColorDialog1.Color
        Else
        End If
    End Sub

    'Color Selector | Main Color
    Private Sub MainColorVar_DoubleClick(sender As Object, e As EventArgs) Handles MainColorVar.DoubleClick
        If ColorDialog1.ShowDialog(Me) Then
            Dim r As Integer = ColorDialog1.Color.R
            Dim b As Integer = ColorDialog1.Color.B
            Dim g As Integer = ColorDialog1.Color.G

            MainColorVar.Text = ("(" + r.ToString + "," + b.ToString + "," + g.ToString + ")")
            MainColorVar.BackColor = ColorDialog1.Color
        Else
        End If
    End Sub

    'Color Selector | Aux Color
    Private Sub AuxColorVar_DoubleClick(sender As Object, e As EventArgs) Handles AuxColorVar.DoubleClick
        If ColorDialog1.ShowDialog(Me) Then
            Dim r As Integer = ColorDialog1.Color.R
            Dim b As Integer = ColorDialog1.Color.B
            Dim g As Integer = ColorDialog1.Color.G

            AuxColorVar.Text = ("(" + r.ToString + "," + b.ToString + "," + g.ToString + ")")
            AuxColorVar.BackColor = ColorDialog1.Color
        Else
        End If
    End Sub

    'Color Settings | Save Button | Seems Fine Needs Error Handling
    Private Sub ColorSettingsSaveTemplateBtn_Click(sender As Object, e As EventArgs) Handles ColorSettingsSaveTemplateBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"

        Dim FileToSave As String = "Color_Settings_" + ColorSettingsTemplateName.Text + ".txt"
        Dim FilePath As String = MyDocumentsFolder + FileToSave

        Dim Data As String = ExplosiveBeamColorVar.Text.ToString + "|" + ExplosiveBeamAuxVar.Text.ToString + "|" + MainColorVar.Text.ToString + "|" + AuxColorVar.Text.ToString + "|" + MainBeamWidthVar.Text.ToString + "|" + AuxBeamWidthVar.Text.ToString

        My.Computer.FileSystem.WriteAllText(FilePath, Data, False)
        ColorSettingsList.Items.Add(FileToSave)
    End Sub

    'Color Settings | Load Template Button | Needs Error Testing
    Private Sub ColorSettingsLoadTemplateBtn_Click(sender As Object, e As EventArgs) Handles ColorSettingsLoadTemplateBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToLoad As String = Nothing
        Dim Data() As String = Nothing

        Try 'Handles System.NullReferenceException if no item is selected in the listbox
            FileToLoad = ColorSettingsList.SelectedItem.ToString
        Catch ex As Exception
            MessageBox.Show("No template file was selected to load!")
        End Try

        Dim FilePath As String = MyDocumentsFolder + FileToLoad

        Try 'Handles System.IO.FileNotFound Exception
            Data = My.Computer.FileSystem.ReadAllText(FilePath).Split("|")
        Catch ex As Exception
            Data = {Nothing, Nothing, Nothing, Nothing, Nothing, Nothing} 'Creates empty data array to prevent errors in data loading
        End Try

        If Not Data(0) Is Nothing Then ExplosiveBeamColorVar.Text = Data(0)
        If Not Data(0) Is Nothing Then ExplosiveBeamColorVar.BackColor = Color.FromArgb(ExplosiveBeamColorVar.Text)
        If Not Data(1) Is Nothing Then ExplosiveBeamAuxVar.Text = Data(1)
        If Not Data(1) Is Nothing Then ExplosiveBeamAuxVar.BackColor = Color.FromArgb(ExplosiveBeamAuxVar.Text)
        If Not Data(2) Is Nothing Then MainColorVar.Text = Data(2)
        If Not Data(2) Is Nothing Then MainColorVar.BackColor = Color.FromArgb(MainColorVar.Text)
        If Not Data(3) Is Nothing Then AuxColorVar.Text = Data(3)
        If Not Data(3) Is Nothing Then AuxColorVar.BackColor = Color.FromArgb(AuxColorVar.Text)
        If Not Data(4) Is Nothing Then MainBeamWidthVar.Text = Data(4)
        If Not Data(5) Is Nothing Then AuxBeamWidthVar.Text = Data(5)
    End Sub

    'Color Settings | Delete Selected Template Button
    Private Sub ColorSettingsDeleteTemplateBtn_Click(sender As Object, e As EventArgs) Handles ColorSettingsDeleteTemplateBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToDelete As String = Nothing
        Try
            FileToDelete = ColorSettingsList.SelectedItem.ToString()
        Catch ex As Exception
            MessageBox.Show("No template file selected for deletion!")
        End Try

        If Not FileToDelete Is Nothing Then
            Dim FilePath As String = MyDocumentsFolder + FileToDelete
            My.Computer.FileSystem.DeleteFile(FilePath)
            ColorSettingsList.Items.Remove(ColorSettingsList.SelectedItem.ToString)
        End If
    End Sub

    'Color Settings | Commit color settings to output file
    Private Sub ColorSettingsConfirmBtn_Click_1(sender As Object, e As EventArgs) Handles ColorSettingsConfirmBtn.Click
        If ColorSettings = True Then
            MessageBox.Show("Error: Color settings already committed!")
        Else
            OutputBox.Text += NewLine + NewLine
        OutputBox.Text += "public static class " + Shortname.ToString + "_Color_Settings" + NewLine + "{" + NewLine
        OutputBox.Text += "public static BeamWeaponColorInfo " + ProjectNamespaces.ToString + " = new BeamWeaponColorInfo(" + NewLine
        OutputBox.Text += "NEWCOLORCODENEDDED" + "," + NewLine
        OutputBox.Text += "NEWCOLORCODENEDDED" + "," + NewLine
        OutputBox.Text += "NEWCOLORCODENEDDED" + "," + NewLine
        OutputBox.Text += "NEWCOLORCODENEDDED" + "," + NewLine
        OutputBox.Text += MainBeamWidthVar.Text.ToString + "," + NewLine
        OutputBox.Text += AuxBeamWidthVar.Text.ToString + NewLine + ");" + NewLine + "}" + NewLine + NewLine
        ColorSettings = True
        End If
    End Sub
#End Region

#Region "Heat Settings"
    'Heat Settings | Reset Button
    Private Sub HeatSettingsResetBtn_Click(sender As Object, e As EventArgs) Handles HeatSettingsResetBtn.Click
        MaxResidualHeatVar.Text = "2000f"
        MaxHeatVar.Text = "1000f"
        ResidualHeatPerTickVar.Text = "10f"
        HeatPerTickVar.Text = "5f"
        ResidualHeatDisPerTickVar.Text = "5f"
        HeatDisPerTickVar.Text = "5f"
        HeatSettingsTemplateName.Text = "DefaultTemplate"
        ResidualHeatIncDelay.Value = "10"
        ResidualHeatDisDelay.Value = "300"
        HeatDissipationDelay.Value = "5"
        KeepAtCharge.Value = "10"
    End Sub

    'Heat Settings | Save Button
    Private Sub HeatSettingsSaveBtn_Click(sender As Object, e As EventArgs) Handles HeatSettingsSaveBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"

        Dim FileToSave As String = "Heat_Settings_" + HeatSettingsTemplateName.Text + ".txt"
        Dim FilePath As String = MyDocumentsFolder + FileToSave

        Dim Data As String = MaxResidualHeatVar.Text.ToString + "|" + MaxHeatVar.Text.ToString + "|" + ResidualHeatPerTickVar.Text.ToString + "|" + HeatPerTickVar.Text.ToString + "|" + ResidualHeatDisPerTickVar.Text.ToString + "|" + HeatDisPerTickVar.Text.ToString + "|" + HeatSettingsTemplateName.Text.ToString + "|" + ResidualHeatIncDelay.Value.ToString + "|" + ResidualHeatDisDelay.Value.ToString + "|" + HeatDissipationDelay.Value.ToString + "|" + KeepAtCharge.Value.ToString

        My.Computer.FileSystem.WriteAllText(FilePath, Data, False)

        If HeatSettingsList.Items.Contains(FileToSave) Then
            MessageBox.Show(FileToSave.ToString + " Has been overwritten successfully!")
        Else
            HeatSettingsList.Items.Add(FileToSave)
        End If
    End Sub

    'Heat Settings | Load Button
    Private Sub HeatSettingsLoadBtn_Click(sender As Object, e As EventArgs) Handles HeatSettingsLoadBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToLoad As String = Nothing
        Dim Data() As String = Nothing

        Try 'Handles System.NullReferenceException if no item is selected in the listbox
            FileToLoad = HeatSettingsList.SelectedItem.ToString
        Catch ex As Exception
            MessageBox.Show("No template file was selected to load!")
        End Try

        Dim FilePath As String = MyDocumentsFolder + FileToLoad

        Try 'Handles System.IO.FileNotFound Exception
            Data = My.Computer.FileSystem.ReadAllText(FilePath).Split("|")
        Catch ex As Exception
            Data = {Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing} 'Creates empty data array to prevent errors in data loading
        End Try

        If Not Data(0) Is Nothing Then MaxResidualHeatVar.Text = Data(0)
        If Not Data(1) Is Nothing Then MaxHeatVar.Text = Data(1)
        If Not Data(2) Is Nothing Then ResidualHeatPerTickVar.Text = Data(2)
        If Not Data(3) Is Nothing Then HeatPerTickVar.Text = Data(3)
        If Not Data(4) Is Nothing Then MainBeamWidthVar.Text = Data(4)
        If Not Data(5) Is Nothing Then ResidualHeatDisPerTickVar.Text = Data(5)
        If Not Data(5) Is Nothing Then HeatDisPerTickVar.Text = Data(6)
        If Not Data(5) Is Nothing Then ResidualHeatIncDelay.Value = Data(7)
        If Not Data(5) Is Nothing Then ResidualHeatDisDelay.Value = Data(8)
        If Not Data(5) Is Nothing Then HeatDissipationDelay.Value = Data(9)
        If Not Data(5) Is Nothing Then KeepAtCharge.Value = Data(10)
    End Sub

    'Heat Settings | Delete Template Button
    Private Sub HeatSettingsDeleteBtn_Click_1(sender As Object, e As EventArgs) Handles HeatSettingsDeleteBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToDelete As String = Nothing
        Try
            FileToDelete = HeatSettingsList.SelectedItem.ToString()
        Catch ex As Exception
            MessageBox.Show("No template file selected for deletion!")
        End Try

        If Not FileToDelete Is Nothing Then
            Dim FilePath As String = MyDocumentsFolder + FileToDelete
            My.Computer.FileSystem.DeleteFile(FilePath)
            HeatSettingsList.Items.Remove(HeatSettingsList.SelectedItem.ToString)
        End If
    End Sub

    'Heat Settings | Commit Button
    Private Sub HeatSettingsCommitBtn_Click(sender As Object, e As EventArgs) Handles HeatSettingsCommitBtn.Click
        If HeatSettings = True Then
            MessageBox.Show("Error: Default settings already committed!")
        Else
            OutputBox.Text += "public static class " + Shortname.ToString + "_Heat_Settings" + NewLine + "{" + NewLine
            OutputBox.Text += "public static BeamWeaponExtendedHeatInfo " + ProjectNamespaces.ToString + " = new BeamWeaponExtendedHeatInfo(" + NewLine
            OutputBox.Text += MaxResidualHeatVar.Text.ToString + "," + NewLine
            OutputBox.Text += MaxHeatVar.Text.ToString + "," + NewLine
            OutputBox.Text += ResidualHeatPerTickVar.Text.ToString + "," + NewLine
            OutputBox.Text += HeatPerTickVar.Text.ToString + "," + NewLine
            OutputBox.Text += ResidualHeatDisPerTickVar.Text.ToString + "," + NewLine
            OutputBox.Text += HeatDisPerTickVar.Text.ToString + "," + NewLine
            OutputBox.Text += ResidualHeatIncDelay.Value.ToString + "," + NewLine
            OutputBox.Text += ResidualHeatDisDelay.Value.ToString + "," + NewLine
            OutputBox.Text += HeatDissipationDelay.Value.ToString + "," + NewLine
            OutputBox.Text += KeepAtCharge.Value.ToString + NewLine + ");" + NewLine + "}" + NewLine + NewLine
            HeatSettings = True
        End If
    End Sub
#End Region

#Region "Default UI Settings"
    'Default UI Settings | Reset Button
    Private Sub DefaultUISettingsResetBtn_Click(sender As Object, e As EventArgs) Handles DefaultUISettingsResetBtn.Click
        '35 Controls
        DefaultUiSettingsTemplateName.Text = "DefaultTemplate"
        OnOffControlEnable.Checked = True
        OnOffControlVisible.Checked = True
        OnOffActionEnabled.Checked = True
        ShootOnceControlEnable.Checked = True
        ShootOnceControlVisible.Checked = True
        ShootOnceActionEnable.Checked = True
        ShootOnOffControlEnable.Checked = True
        ShootOnOffControlVisible.Checked = True
        ShootOnOffActionEnabled.Checked = True
        EnableIdleMovementControlEnabled.Checked = True
        EnableIdleMovementControlVisible.Checked = True
        EnableIdleMovementActionEnabled.Checked = True
        TargetMissilesControlEnabled.Checked = True
        TargetMissilesControlVisible.Checked = True
        TargetMissilesActionEnabled.Checked = True
        TargetMeteorsControlEnabled.Checked = True
        TargetMeteorsControlVisible.Checked = True
        TargetMeteorsActionEnabled.Checked = True
        TargetSmallShipsControlEnabled.Checked = True
        TargetSmallShipsControlVisible.Checked = True
        TargetSmallShipsActionEnabled.Checked = True
        TargetLargeShipsControlEnabled.Checked = True
        TargetLargeShipsControlVisible.Checked = True
        TargetLargeShipsActionEnabled.Checked = True
        TargetLargeShipsActionEnabled.Checked = True
        TargetStationsControlEnabled.Checked = True
        TargetStationsControlVisible.Checked = True
        TargetStationsActionEnabled.Checked = True
        TargetCharactersControlEnabled.Checked = True
        TargetCharactersControlVisible.Checked = True
        TargetCharactersActionEnabled.Checked = True
        TargetNeutralsControlEnabled.Checked = True
        TargetNuetralsControlVisible.Checked = True
        TargetNeutralsActionEnabled.Checked = True
        ControlControlVisible.Checked = True
        ControlActionEnabled.Checked = True
    End Sub

    'Default UI Settings | Save Button
    Private Sub DefaultUiSettingsSaveBtn_Click(sender As Object, e As EventArgs) Handles DefaultUiSettingsSaveBtn.Click
        Dim Bool1 As Boolean = OnOffControlEnable.CheckState
        Dim Bool2 As Boolean = OnOffControlVisible.CheckState
        Dim Bool3 As Boolean = OnOffActionEnabled.CheckState
        Dim Bool4 As Boolean = ShootOnceControlEnable.CheckState
        Dim Bool5 As Boolean = ShootOnceControlVisible.CheckState
        Dim Bool6 As Boolean = ShootOnceActionEnable.CheckState
        Dim Bool7 As Boolean = ShootOnOffControlEnable.CheckState
        Dim Bool8 As Boolean = ShootOnOffControlVisible.CheckState
        Dim Bool9 As Boolean = ShootOnOffActionEnabled.CheckState
        Dim Bool10 As Boolean = EnableIdleMovementControlEnabled.CheckState
        Dim Bool11 As Boolean = EnableIdleMovementControlVisible.CheckState
        Dim Bool12 As Boolean = EnableIdleMovementActionEnabled.CheckState
        Dim Bool13 As Boolean = TargetMissilesControlEnabled.CheckState
        Dim Bool14 As Boolean = TargetMissilesControlVisible.CheckState
        Dim Bool15 As Boolean = TargetMissilesActionEnabled.CheckState
        Dim Bool16 As Boolean = TargetMeteorsControlEnabled.CheckState
        Dim Bool17 As Boolean = TargetMeteorsControlVisible.CheckState
        Dim Bool18 As Boolean = TargetMeteorsActionEnabled.CheckState
        Dim Bool19 As Boolean = TargetSmallShipsControlEnabled.CheckState
        Dim Bool20 As Boolean = TargetSmallShipsControlVisible.CheckState
        Dim Bool21 As Boolean = TargetSmallShipsActionEnabled.CheckState
        Dim Bool22 As Boolean = TargetLargeShipsControlEnabled.CheckState
        Dim Bool23 As Boolean = TargetLargeShipsControlVisible.CheckState
        Dim Bool24 As Boolean = TargetLargeShipsActionEnabled.CheckState
        Dim Bool25 As Boolean = TargetStationsControlEnabled.CheckState
        Dim Bool26 As Boolean = TargetStationsControlVisible.CheckState
        Dim Bool27 As Boolean = TargetStationsActionEnabled.CheckState
        Dim Bool28 As Boolean = TargetCharactersControlEnabled.CheckState
        Dim Bool29 As Boolean = TargetCharactersControlVisible.CheckState
        Dim Bool30 As Boolean = TargetCharactersActionEnabled.CheckState
        Dim Bool31 As Boolean = TargetNeutralsControlEnabled.CheckState
        Dim Bool32 As Boolean = TargetNuetralsControlVisible.CheckState
        Dim Bool33 As Boolean = TargetNeutralsActionEnabled.CheckState
        Dim Bool34 As Boolean = ControlControlVisible.CheckState
        Dim Bool35 As Boolean = ControlActionEnabled.CheckState

        Dim Data As String = Bool1.ToString + "|" + Bool2.ToString + "|" + Bool3.ToString + "|" + Bool4.ToString + "|" + Bool5.ToString + "|" + Bool6.ToString + "|" + Bool7.ToString + "|" + Bool8.ToString + "|" + Bool9.ToString + "|" + Bool10.ToString + "|" + Bool11.ToString + "|" + Bool12.ToString + "|" + Bool13.ToString + "|" + Bool14.ToString + "|" + Bool15.ToString + "|" + Bool16.ToString + "|" + Bool17.ToString + "|" + Bool18.ToString + "|" + Bool19.ToString + "|" + Bool20.ToString + "|" + Bool21.ToString + "|" + Bool22.ToString + "|" + Bool23.ToString + "|" + Bool24.ToString + "|" + Bool25.ToString + "|" + Bool26.ToString + "|" + Bool27.ToString + "|" + Bool28.ToString + "|" + Bool29.ToString + "|" + Bool30.ToString + "|" + Bool31.ToString + "|" + Bool32.ToString + "|" + Bool33.ToString + "|" + Bool34.ToString + "|" + Bool35.ToString
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToSave As String = "DefaultUI_Settings_" + DefaultUiSettingsTemplateName.Text + ".txt"
        Dim FilePath As String = MyDocumentsFolder + FileToSave
        My.Computer.FileSystem.WriteAllText(FilePath, Data, False)

        If DefaultUISettingsList.Items.Contains(FileToSave) Then
            MessageBox.Show(FileToSave.ToString + " Has been overwritten successfully!")
        Else
            DefaultUISettingsList.Items.Add(FileToSave)
        End If
    End Sub

    'Default UI Settings | Load Button
    Private Sub DefaultUiSettingsLoadBtn_Click(sender As Object, e As EventArgs) Handles DefaultUiSettingsLoadBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToLoad As String = Nothing
        Dim Data() As String = Nothing

        Try 'Handles System.NullReferenceException if no item is selected in the listbox
            FileToLoad = DefaultUISettingsList.SelectedItem.ToString
        Catch ex As Exception
            MessageBox.Show("No template file was selected to load!")
        End Try

        Dim FilePath As String = MyDocumentsFolder + FileToLoad

        Try 'Handles System.IO.FileNotFound Exception
            Data = My.Computer.FileSystem.ReadAllText(FilePath).Split("|")
        Catch ex As Exception
            Data = {Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing} 'Creates empty data array to prevent errors in data loading
        End Try

        OnOffControlEnable.Checked = Data(0)
        OnOffControlVisible.Checked = Data(1)
        OnOffActionEnabled.Checked = Data(2)
        ShootOnceControlEnable.Checked = Data(3)
        ShootOnceControlVisible.Checked = Data(4)
        ShootOnceActionEnable.Checked = Data(5)
        ShootOnOffControlEnable.Checked = Data(6)
        ShootOnOffControlVisible.Checked = Data(7)
        ShootOnOffActionEnabled.Checked = Data(8)
        EnableIdleMovementControlEnabled.Checked = Data(9)
        EnableIdleMovementControlVisible.Checked = Data(10)
        EnableIdleMovementActionEnabled.Checked = Data(11)
        TargetMissilesControlEnabled.Checked = Data(12)
        TargetMissilesControlVisible.Checked = Data(13)
        TargetMissilesActionEnabled.Checked = Data(14)
        TargetMeteorsControlEnabled.Checked = Data(15)
        TargetMeteorsControlVisible.Checked = Data(16)
        TargetMeteorsActionEnabled.Checked = Data(17)
        TargetSmallShipsControlEnabled.Checked = Data(18)
        TargetSmallShipsControlVisible.Checked = Data(19)
        TargetSmallShipsActionEnabled.Checked = Data(20)
        TargetLargeShipsControlEnabled.Checked = Data(21)
        TargetLargeShipsControlVisible.Checked = Data(22)
        TargetLargeShipsActionEnabled.Checked = Data(23)
        TargetStationsControlEnabled.Checked = Data(24)
        TargetStationsControlVisible.Checked = Data(25)
        TargetStationsActionEnabled.Checked = Data(26)
        TargetCharactersControlEnabled.Checked = Data(27)
        TargetCharactersControlVisible.Checked = Data(28)
        TargetCharactersActionEnabled.Checked = Data(29)
        TargetNeutralsControlEnabled.Checked = Data(30)
        TargetNuetralsControlVisible.Checked = Data(31)
        TargetNeutralsActionEnabled.Checked = Data(32)
        ControlControlVisible.Checked = Data(33)
        ControlActionEnabled.Checked = Data(34)
    End Sub

    'Default UI Settings | Delete Button
    Private Sub DefaultUiSettingsDeleteBtn_Click(sender As Object, e As EventArgs) Handles DefaultUiSettingsDeleteBtn.Click
        Dim MyDocumentsFolder As String = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "/SEWeaponManager/"
        Dim FileToDelete As String = Nothing
        Try
            FileToDelete = DefaultUISettingsList.SelectedItem.ToString()
        Catch ex As Exception
            MessageBox.Show("No template file selected for deletion!")
        End Try

        If Not FileToDelete Is Nothing Then
            Dim FilePath As String = MyDocumentsFolder + FileToDelete
            My.Computer.FileSystem.DeleteFile(FilePath)
            DefaultUISettingsList.Items.Remove(DefaultUISettingsList.SelectedItem.ToString)
        End If
    End Sub

    'Default UI Settings | Commit Button
    Private Sub DefaultUiSettingsCommitBtn_Click(sender As Object, e As EventArgs) Handles DefaultUiSettingsCommitBtn.Click
        If DefaultUISettings = True Then
            MessageBox.Show("Default UI Settings have already been comitted!")
        Else
            Dim Bool1 As Boolean = OnOffControlEnable.CheckState
            Dim Bool2 As Boolean = OnOffControlVisible.CheckState
            Dim Bool3 As Boolean = OnOffActionEnabled.CheckState
            Dim Bool4 As Boolean = ShootOnceControlEnable.CheckState
            Dim Bool5 As Boolean = ShootOnceControlVisible.CheckState
            Dim Bool6 As Boolean = ShootOnceActionEnable.CheckState
            Dim Bool7 As Boolean = ShootOnOffControlEnable.CheckState
            Dim Bool8 As Boolean = ShootOnOffControlVisible.CheckState
            Dim Bool9 As Boolean = ShootOnOffActionEnabled.CheckState
            Dim Bool10 As Boolean = EnableIdleMovementControlEnabled.CheckState
            Dim Bool11 As Boolean = EnableIdleMovementControlVisible.CheckState
            Dim Bool12 As Boolean = EnableIdleMovementActionEnabled.CheckState
            Dim Bool13 As Boolean = TargetMissilesControlEnabled.CheckState
            Dim Bool14 As Boolean = TargetMissilesControlVisible.CheckState
            Dim Bool15 As Boolean = TargetMissilesActionEnabled.CheckState
            Dim Bool16 As Boolean = TargetMeteorsControlEnabled.CheckState
            Dim Bool17 As Boolean = TargetMeteorsControlVisible.CheckState
            Dim Bool18 As Boolean = TargetMeteorsActionEnabled.CheckState
            Dim Bool19 As Boolean = TargetSmallShipsControlEnabled.CheckState
            Dim Bool20 As Boolean = TargetSmallShipsControlVisible.CheckState
            Dim Bool21 As Boolean = TargetSmallShipsActionEnabled.CheckState
            Dim Bool22 As Boolean = TargetLargeShipsControlEnabled.CheckState
            Dim Bool23 As Boolean = TargetLargeShipsControlVisible.CheckState
            Dim Bool24 As Boolean = TargetLargeShipsActionEnabled.CheckState
            Dim Bool25 As Boolean = TargetStationsControlEnabled.CheckState
            Dim Bool26 As Boolean = TargetStationsControlVisible.CheckState
            Dim Bool27 As Boolean = TargetStationsActionEnabled.CheckState
            Dim Bool28 As Boolean = TargetCharactersControlEnabled.CheckState
            Dim Bool29 As Boolean = TargetCharactersControlVisible.CheckState
            Dim Bool30 As Boolean = TargetCharactersActionEnabled.CheckState
            Dim Bool31 As Boolean = TargetNeutralsControlEnabled.CheckState
            Dim Bool32 As Boolean = TargetNuetralsControlVisible.CheckState
            Dim Bool33 As Boolean = TargetNeutralsActionEnabled.CheckState
            Dim Bool34 As Boolean = ControlControlVisible.CheckState
            Dim Bool35 As Boolean = ControlActionEnabled.CheckState

            OutputBox.Text += "public static class " + Shortname.ToString + "_Default_Ui_Settings" + NewLine + "{" + NewLine
            OutputBox.Text += "public static BeamWeaponUiInfo " + ProjectNamespaces.ToString + " = new BeamWeaponUiInfo(" + NewLine
            OutputBox.Text += Bool1.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool2.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool3.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool4.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool5.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool6.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool7.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool8.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool9.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool10.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool11.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool12.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool13.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool14.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool15.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool16.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool17.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool18.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool19.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool20.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool21.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool22.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool23.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool24.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool25.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool26.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool27.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool28.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool29.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool30.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool31.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool32.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool33.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool34.ToString.ToLower + "," + NewLine
            OutputBox.Text += Bool35.ToString.ToLower + NewLine + ");" + NewLine + "}" + NewLine + NewLine
            DefaultUISettings = True
        End If
    End Sub
#End Region


    'Export Output File
    Private Sub MdButton13_Click(sender As Object, e As EventArgs) Handles MdButton13.Click
        If SaveFileDialog1.ShowDialog(Me) Then
            Dim FileSavePath As String = SaveFileDialog1.FileName.ToString

            My.Computer.FileSystem.WriteAllText(FileSavePath + ".cs", OutputBox.Text, False)
        End If
    End Sub

    Private Sub OutputResetBtn_Click(sender As Object, e As EventArgs) Handles OutputResetBtn.Click
        OutputBox.Text = ""
        DefaultSettings = False
        ColorSettings = False
        HeatSettings = False
        DefaultUISettings = False
        CustomUISettings = False
        BarrelSettings = False

        MessageBox.Show("Output File Reset!")
    End Sub
End Class
