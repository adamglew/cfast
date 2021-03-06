Public Class ThermalProperty
    Friend Const MaximumProperties As Integer = 125
    ' All units within the class are assumed to be consistent and typically SI
    Private HasErrors As Integer                ' Temp variable that holds error count during error check
    Private aChanged As Boolean                 ' Indicates if record has been changed


    Private aShortName As String                ' One word name for material
    Private aName As String                     ' Material Name
    Private aConductivity As Double             ' Thermal conductivity
    Private aSpecificHeat As Double             ' Specific Heat
    Private aDensity As Double                  ' Density in kg/m^3
    Private aThickness As Double                ' Material Thickness in m
    Private aEmissivity As Double               ' Emissivity
    Private Const HClDim As Integer = 6              '
    Private aHClDeposition(HClDim) As Double    ' HCl Deposition Coefficients
    Private aFYI As String                  ' Descriptor for additional user supplied information
    Public Sub New()
        aShortName = ""
        aName = ""
        aFYI = ""
        aConductivity = 0.16
        aSpecificHeat = 900.0
        aDensity = 790.0
        aThickness = 0.016
        aEmissivity = 0.9
        Dim i As Integer
        For i = 0 To HClDim
            aHClDeposition(i) = 0.0
        Next
        aChanged = True
    End Sub
    Public Sub New(ByVal ShortName As String, ByVal Name As String, ByVal Conductivity As Double, ByVal SpecificHeat As Double, _
    ByVal Density As Double, ByVal Thickness As Double, ByVal Emissivity As Double)
        Me.ShortName = ShortName
        Me.Name = Name
        Me.Conductivity = Conductivity
        Me.SpecificHeat = SpecificHeat
        Me.Density = Density
        Me.Thickness = Thickness
        Me.Emissivity = Emissivity
        Dim i As Integer
        For i = 0 To HClDim
            HClDeposition(i) = 0.0
        Next
        aChanged = True
    End Sub
    Public Sub GetThermalProperties(ByRef ShortName As String, ByRef Name As String, ByRef Conductivity As Double, ByRef SpecificHeat As Double, _
    ByRef Density As Double, ByRef Thickness As Double, ByRef Emissivity As Double)
        ShortName = Me.ShortName
        Name = Me.Name
        Conductivity = Me.Conductivity
        SpecificHeat = Me.SpecificHeat
        Density = Me.Density
        Thickness = Me.Thickness
        Emissivity = Me.Emissivity
    End Sub
    Friend Property ShortName() As String
        Get
            Return aShortName
        End Get
        Set(ByVal Value As String)
            If Value <> aShortName Then
                aShortName = Value
                aChanged = True
            End If
        End Set
    End Property
    Friend Property Name() As String
        Get
            Return aName
        End Get
        Set(ByVal Value As String)
            If Value <> aName Then
                aName = Value
                aChanged = True
            End If
        End Set
    End Property
    Friend Property HClDeposition(ByVal i As Integer) As Double
        Get
            Return aHClDeposition(i)
        End Get
        Set(ByVal Value As Double)
            If Value <> aHClDeposition(i) Then
                aHClDeposition(i) = Value
                aChanged = True
            End If
        End Set
    End Property
    Friend Property Emissivity() As Double
        Get
            Return aEmissivity
        End Get
        Set(ByVal Value As Double)
            If Value <> aEmissivity Then
                aEmissivity = Value
                aChanged = True
            End If
        End Set
    End Property
    Friend Property Thickness() As Double
        Get
            Return myUnits.Convert(UnitsNum.Length).FromSI(aThickness)
        End Get
        Set(ByVal Value As Double)
            If myUnits.Convert(UnitsNum.Length).ToSI(Value) <> aThickness Then
                aThickness = myUnits.Convert(UnitsNum.Length).ToSI(Value)
                aChanged = True
            End If
        End Set
    End Property
    Friend Property Density() As Double
        Get
            Return myUnits.Convert(UnitsNum.Density).FromSI(aDensity)
        End Get
        Set(ByVal Value As Double)
            If myUnits.Convert(UnitsNum.Density).ToSI(Value) <> aDensity Then
                aDensity = myUnits.Convert(UnitsNum.Density).ToSI(Value)
                aChanged = True
            End If
        End Set
    End Property
    Friend Property SpecificHeat() As Double
        Get
            Return myUnits.Convert(UnitsNum.SpecificHeat).FromSI(aSpecificHeat)
        End Get
        Set(ByVal Value As Double)
            If myUnits.Convert(UnitsNum.SpecificHeat).ToSI(Value) <> aSpecificHeat Then
                aSpecificHeat = myUnits.Convert(UnitsNum.SpecificHeat).ToSI(Value)
                aChanged = True
            End If
        End Set
    End Property
    Friend Property Conductivity() As Double
        Get
            Return myUnits.Convert(UnitsNum.Conductivity).FromSI(aConductivity)
        End Get
        Set(ByVal Value As Double)
            If myUnits.Convert(UnitsNum.Conductivity).ToSI(Value) <> aConductivity Then
                aConductivity = myUnits.Convert(UnitsNum.Conductivity).ToSI(Value)
                aChanged = True
            End If
        End Set
    End Property
    Public Property FYI() As String
        Get
            Return aFYI
        End Get
        Set(ByVal Value As String)
            If Value <> aFYI Then
                aChanged = True
                aFYI = Value
            End If
        End Set
    End Property
    Friend Property Changed() As Boolean
        Get
            Return aChanged
        End Get
        Set(ByVal Value As Boolean)
            aChanged = Value
        End Set
    End Property
    Public Sub GetHCl(ByRef HClCoefficients() As Double)
        Dim i As Integer
        ReDim HClCoefficients(HClDim)
        For i = 0 To HClDim
            HClCoefficients(i) = HClDeposition(i)
        Next
    End Sub
    Public Sub SetHCl(ByVal HClCoefficients() As Double)
        Dim i As Integer
        If HClCoefficients.GetUpperBound(0) = HClDim Then
            For i = 0 To HClDim
                HClDeposition(i) = HClCoefficients(i)
            Next
        End If
        aChanged = True
    End Sub
    Public ReadOnly Property IsValid(ByVal ThermalPropertyNumber As Integer) As String
        ' Checks the overall validity of the current thermal property specification
        Get
            myUnits.SI = True
            HasErrors = 0
            If Len(aShortName) < 1 Or Len(ShortName) > 16 Then
                myErrors.Add("Thermal Property " + aName + ". Short name is blank or more than 16 characters long", ErrorMessages.TypeError)
                HasErrors += 1
            End If
            If aEmissivity < 0.0 Or aEmissivity > 1.0 Then
                myErrors.Add("Thermal Property " + aName + ". Emissivity is less than 0 or greater than 1", ErrorMessages.TypeError)
                HasErrors += 1
            End If

            If aConductivity < 0.0 Then
                myErrors.Add("Thermal Property " + aName + ". Conductivity is less than 0", ErrorMessages.TypeError)
                HasErrors += 1
            End If
            If aSpecificHeat < 0.0 Then
                myErrors.Add("Thermal Property " + aName + ". Specific heat is less than 0", ErrorMessages.TypeError)
                HasErrors += 1
            End If
            If aDensity < 0.0 Then
                myErrors.Add("Thermal Property " + aName + ". Density is less than 0", ErrorMessages.TypeError)
                HasErrors += 1
            End If
            If aThickness < 0.0 Then
                myErrors.Add("Thermal Property " + aName + ". Thickness is less than 0", ErrorMessages.TypeError)
                HasErrors += 1
            End If
            myUnits.SI = False
            Return HasErrors
        End Get
    End Property
End Class
Public Class ThermalPropertiesCollection
    Inherits System.Collections.CollectionBase
    Public ReadOnly Maximum As Integer = ThermalProperty.MaximumProperties
    Private i As Integer, FoundIndex As Integer, j As Integer
    Private HasErrors As Integer
    Private aFileName As String = "thermal"
    Private aFileChanged As Boolean = False
    Property FileName() As String
        Get
            Return System.IO.Path.GetFileNameWithoutExtension(aFileName)
        End Get
        Set(ByVal Value As String)
            If Value <> aFileName Then
                aFileName = System.IO.Path.GetFileNameWithoutExtension(Value) + ".csv"
                aFileChanged = True
            End If
        End Set
    End Property
    Property FileChanged() As Boolean
        Get
            Return aFileChanged
        End Get
        Set(ByVal Value As Boolean)
            aFileChanged = Value
        End Set
    End Property
    Public ReadOnly Property Changed() As Boolean
        Get
            If Count > 0 Then
                Dim aThermal As ThermalProperty
                For i = 0 To Count - 1
                    aThermal = CType(List.Item(i), ThermalProperty)
                    If aThermal.Changed Then Return True
                Next
            End If
            If FileChanged = True Then Return True
            Return False
        End Get
    End Property
    Public ReadOnly Property ValidThermalProperty(ByVal Name As String, ByVal Location As String) As String
        Get
            Dim index As Integer, aThermalProperty As New ThermalProperty
            index = GetIndex(Name)
            If index <= -1 Then
                Dim TempThermalProperties As New ThermalPropertiesCollection
                ReadThermalProperties(Application.StartupPath.ToString + "\" + "thermal.csv", TempThermalProperties)
                ReadThermalProperties(".\" + "thermal.csv", TempThermalProperties)
                If TempThermalProperties.GetIndex(Name) >= 0 Then
                    aThermalProperty = TempThermalProperties.Item(TempThermalProperties.GetIndex(Name))
                    myThermalProperties.Add(aThermalProperty)
                Else
                    If Name <> "OFF" And Name <> "Off" Then
                        myErrors.Add("Thermal property " + Name + " used as a " + Location + " does not exist", ErrorMessages.TypeFatal)
                    End If
                End If
            End If
            Return Name
        End Get
    End Property
    Public Sub Add(ByVal aThermal As ThermalProperty)
        Dim i As Integer
        If Count > 0 Then
            For i = 0 To Count - 1
                If aThermal.ShortName = Item(i).ShortName Then
                    Exit Sub
                End If
            Next
        End If
        List.Add(aThermal)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        ' make sure that the compartment number is valid
        If index > Count - 1 Or index < 0 Then
            System.Windows.Forms.MessageBox.Show("Internal Error (User should not see this). Thermal property number not found.")
        Else
            List.RemoveAt(index)
        End If
    End Sub
    Public Sub Copy(ByVal indexFrom As Integer, ByVal indexTo As Integer)
        Dim FromMaterial As New ThermalProperty, ToMaterial As New ThermalProperty
        FromMaterial = CType(List.Item(indexFrom), ThermalProperty)
        ToMaterial.ShortName = FromMaterial.ShortName
        ToMaterial.Name = FromMaterial.Name
        ToMaterial.Conductivity = FromMaterial.Conductivity
        ToMaterial.SpecificHeat = FromMaterial.SpecificHeat
        ToMaterial.Density = FromMaterial.Density
        ToMaterial.Thickness = FromMaterial.Thickness
        ToMaterial.Emissivity = FromMaterial.Emissivity
        Dim Vector1() As Double = {0}
        FromMaterial.GetHCl(Vector1)
        ToMaterial.SetHCl(Vector1)
        List.Item(indexTo) = ToMaterial
    End Sub
    Default Public Property Item(ByVal index As Integer) As ThermalProperty
        Get
            If index > Count - 1 Or index < 0 Then
                System.Windows.Forms.MessageBox.Show("Internal Error (User should not see this). Thermal property number not found.")
                ' These are just to eliminate a compile warning.  If we get here, we're in trouble anyway
                Dim aThermalProperty As New ThermalProperty
                Return aThermalProperty
            Else
                Return CType(List.Item(index), ThermalProperty)
            End If
        End Get
        Set(ByVal Value As ThermalProperty)
            If index > Count - 1 Or index < 0 Then
                System.Windows.Forms.MessageBox.Show("Internal Error (User should not see this). Thermal property number not found.")
            Else
                List.Item(index) = Value
            End If
        End Set
    End Property
    Friend ReadOnly Property MaterialsList() As String
        Get
            Dim list As String = "Off|"
            If Count > 0 Then
                For i = 0 To Count - 1
                    list += Item(i).Name
                    If i < Count - 1 Then list += "|"
                Next
            End If
            Return list
        End Get
    End Property
    Public ReadOnly Property GetLongName(ByVal Shortname As String) As String
        Get
            If Count > 0 Then
                For i = 0 To Count - 1
                    If Item(i).ShortName = Shortname Then
                        Return Item(i).Name
                        Exit Property
                    End If
                Next
                Return "Off"
            Else
                Return "Off"
            End If
        End Get
    End Property
    Public ReadOnly Property GetShortName(ByVal aName As String) As String
        Get
            FoundIndex = -1
            If Count > 0 Then
                For i = 0 To Count - 1
                    If Me(i).Name = aName Then
                        Return Me(i).ShortName
                        Exit Property
                    End If
                Next
            End If
            Return "Off"
        End Get
    End Property
    Public ReadOnly Property GetIndex(ByVal Shortname As String) As Integer
        Get
            If Count > 0 Then
                For i = 0 To Count - 1
                    If Item(i).ShortName = Shortname Then
                        Return i
                        Exit Property
                    End If
                Next
            End If
            If Shortname.ToUpper = "OFF" Then
                Return -1
            End If
            Return -2
        End Get
    End Property
    Public ReadOnly Property NumberofConnections(ByVal aShortName As String) As Integer
        Get
            Dim numUses As Integer = 0
            If myCompartments.Count > 0 Then
                Dim aCompartment As Compartment
                For i = 0 To myCompartments.Count - 1
                    aCompartment = myCompartments.Item(i)
                    For j = 1 To 3
                        If aCompartment.CeilingMaterial(j) = aShortName Then numUses += 1
                        If aCompartment.FloorMaterial(j) = aShortName Then numUses += 1
                        If aCompartment.WallMaterial(j) = aShortName Then numUses += 1
                    Next
                Next
            End If
            If myTargets.Count > 0 Then
                Dim aTarget As New Target
                For i = 0 To myTargets.Count - 1
                    aTarget = myTargets.Item(i)
                    If aTarget.Material = aShortName Then numUses += 1
                Next
            End If
            Return numUses
        End Get
    End Property
    Public ReadOnly Property IsValid() As Integer
        Get
            HasErrors = 0
            ' Check individual fire objects or instances for errors
            If Count > 0 Then
                Dim aThermal1 As ThermalProperty, aThermal2 As ThermalProperty
                For i = 0 To Count - 1
                    aThermal1 = CType(List(i), ThermalProperty)
                    HasErrors += aThermal1.IsValid(i + 1)
                Next
                ' Cannot have duplicate thermal property names
                If Count > 1 Then
                    For i = 0 To myThermalProperties.Count - 2
                        aThermal1 = myThermalProperties(i)
                        For j = i + 1 To myThermalProperties.Count - 1
                            aThermal2 = myThermalProperties(j)
                            If aThermal1.ShortName = aThermal2.ShortName Then
                                myErrors.Add(aThermal1.ShortName + " is used more than once as a thermal property short name. Duplicate names are not allowed.", ErrorMessages.TypeFatal)
                                HasErrors += 1
                            End If
                        Next
                    Next
                End If
            End If
            Return HasErrors
        End Get
    End Property
End Class