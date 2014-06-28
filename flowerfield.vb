Imports System.Windows.Forms
Imports System.Collections.CollectionBase 'used in buttonarray
Imports System.Resources 'image load
Imports System.Timers
'tram

Public Class Test
    Inherits System.Windows.Forms.Form
    
    dim Xsize as integer = 20
    dim Ysize as integer = 20
    
    private b as New Button()
    private c as New Button()
    private d as New Button()
    
    Private Shared aTimer As System.Timers.Timer
    
    'Dim MyControlArray as ButtonArray
    
    public mybuttarray(Xsize,Ysize) as button
     
    
    Public Sub New()
        MyBase.New()
        MyBase.Topmost = True
        MyBase.Text = "Это Заголовок формы"
        MyBase.Size = new System.Drawing.Size(520,520)
        
       ' MyControlArray = New ButtonArray(Me)
       ' MyControlArray.AddNewButton()
       ' MyControlArray(0).BackColor = System.Drawing.Color.Red
        
        b.Dock = DockStyle.Top
        b.Text = "Это Кнопка b"
        addhandler b.Click, addressof b_click    

        c.Dock = DockStyle.Top
        c.Text = "Это Кнопка c"
        addhandler c.Click, addressof c_click         

        d.Dock = DockStyle.Top
        d.Text = "Это Кнопка d"
        addhandler d.Click, addressof d_click      
        
        
        MyBase.Controls.Add(d)
        MyBase.Controls.Add(c)
        MyBase.Controls.Add(b)
        
        
        MyBase.Show()                                
            
        b.PerformClick()  
        
            
    End sub


    public sub settimer
        ' Create a timer with a ten second interval.
        aTimer = New System.Timers.Timer(10000)

        ' Hook up the Elapsed event for the timer. 
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent

        ' Set the Interval to 2 seconds (2000 milliseconds).
        aTimer.Interval = 40
        
        aTimer.SynchronizingObject = me
    
    end sub

    

    
    public sub gobuttons
    dim i,j as integer
    
    for i = 0 to Xsize
    
        for j = 0 to Ysize
            
            'mybuttarray(i,j) = Nothing
            
            if mybuttarray(i,j) is Nothing then 
                console.write (". Nothing, i=" & CStr(i) & ";j=" & CStr(j))
                mybuttarray(i,j) = new button()
                mybuttarray(i,j).location = new System.Drawing.Point(20 + i*16, 70 + j*16)
                mybuttarray(i,j).Name = ""
                mybuttarray(i,j).Tag = i*(Ysize+1)+j
                mybuttarray(i,j).Size = new System.Drawing.Size(16,16)
                
                MyBase.Controls.Add(mybuttarray(i,j))
                addhandler mybuttarray(i,j).MouseDown, addressof butt_clicker 
            else
                'console.write (". i=" & CStr(i) & ";j=" & CStr(j))
            end if
            
            
            mybuttarray(i,j).FlatStyle = FlatStyle.Standard
            
            if (i-10)*(i-10)+(j-10)*(j-10)<57 then 
               if (i-10)*(i-10)+(j-10)*(j-10)>23 then 
                    mybuttarray(i,j).Image = system.drawing.image.fromfile("smile.png")
               else
                    mybuttarray(i,j).Image = system.drawing.image.fromfile("flower.png")
               end if
            else
                    mybuttarray(i,j).Image = system.drawing.image.fromfile("dot.png")            
            end if
            

            
        next j
    
    next i
    
    end sub
   
   
    public function randomValue(byref lowerbound as integer,byref upperbound as integer) as integer
        randomValue = CInt(Math.Floor((upperbound - lowerbound + 1) * Rnd())) + lowerbound
    end function    
    
    public sub randbuttons
    dim i,j as integer
    
    for i = 0 to Xsize
            
            j = randomvalue(0,Ysize)
            
            mybuttarray(i,j).FlatStyle = FlatStyle.Popup
            mybuttarray(i,j).Image = system.drawing.image.fromfile("dog.png")
    
    next i
    
    end sub


    
    'dog mover
    public sub dogmove
    dim i,j,di,dj as integer
    
    for i = 0 to Xsize
        for j = 0 to Ysize
        
           if mybuttarray(i,j).FlatStyle = FlatStyle.Popup then
                console.write(  vbCrlf & " doggy at (" & i & "," & j & ")")
                di = randomvalue(-1,1)                
                if i+di>-1 and i+di<Xsize+1 then
                    dj = randomvalue(-1,1)
                    if j+dj>-1 and j+dj<Ysize+1 then
                        'move the dog
                        if mybuttarray(i+di,j+dj).FlatStyle <> FlatStyle.Popup then
                            mybuttarray(i,j).Image = Nothing
                            mybuttarray(i,j).FlatStyle = FlatStyle.Standard          
                            
                            console.writeline(mybuttarray(i,j).BackColor.ToArgb())
                            mybuttarray(i,j).BackColor = System.Drawing.Color.FromArgb(mybuttarray(i,j).BackColor.ToArgb()+100+100*256+100*256*256)
                            
                            mybuttarray(i+di,j+dj).FlatStyle = FlatStyle.System                                                        
                            console.write(" doh jumped (" & di & ","& dj & "); now at ("& i+di &","& j+dj &")"  )
                        end if
                    end if
                end if
           end if
                
        next j
    next i

    for i = 0 to Xsize
        for j = 0 to Ysize
            if mybuttarray(i,j).FlatStyle = FlatStyle.System then
                'paint the dog
                mybuttarray(i,j).FlatStyle = FlatStyle.Popup
                mybuttarray(i,j).Image = system.drawing.image.fromfile("dog.png")
            end if
        next j
    next i
   
    end sub

    
    
    Public Sub butt_clicker(ByVal sender As Object, ByVal e As _
        MouseEventArgs)
       'System.EventArgs)
       'MessageBox.Show("you have clicked button " & CType(CType(sender, _
        Console.Writeline("you have clicked button " & CType(CType(sender, _
          System.Windows.Forms.Button).Tag, String))
       dim zis as button
       zis = CType(sender, _
          System.Windows.Forms.Button)
       
       zis.FlatStyle = FlatStyle.Popup
       zis.Image = system.drawing.image.fromfile("dog.png")
          
#If False Then
       if zis.FlatStyle = FlatStyle.system then             
            zis.Image = system.drawing.image.fromfile("cat.png")
            zis.FlatStyle = FlatStyle.Standard
       elseif  zis.FlatStyle = FlatStyle.Flat then
            'zis.Image  = nothing       
            zis.FlatStyle = FlatStyle.system
       elseif  zis.Flatstyle = FlatStyle.Popup then
            zis.Flatstyle = FlatStyle.Flat
       else
            zis.Flatstyle = FlatStyle.Popup
       end if
#End If          

       
    End Sub
    
    
    public sub b_click(ByVal sender as object, byval e as eventargs)
        console.writeline("b_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbTab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
       ' MyControlArray.Remove()
       call gobuttons
       call randbuttons
       call settimer
    end sub
    
    public sub c_click(ByVal sender as object, byval e as eventargs)
       console.writeline("c_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbtab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))       
       call dogmove
    end sub
    
    public sub d_click(ByVal sender as object, byval e as eventargs)
       console.writeline("d_click executed" & vbCrlf & "on object : " & sender.tostring() & vbCrlf & vbtab  & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))       
       if aTimer.Enabled = True then
            aTimer.Enabled = False
       else 
            aTimer.Enabled = True     
       end if
       
    end sub    
    
    
    ' Specify what you want to happen when the Elapsed event is  
    ' raised. 
    
    public Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime)
        call dogmove
    End Sub 
    
   
    
    Public Shared Sub Main()
        'Dim T as New Test
        'T = New Test()
        System.Windows.Forms.Application.Run(New Test())
    End Sub
    
End Class


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public Class ButtonArray
   Inherits System.Collections.CollectionBase
   
   private HostForm as System.Windows.Forms.Form
   
   Public Function AddNewButton() As System.Windows.Forms.Button
       ' Create a new instance of the Button class.
       Dim aButton As New System.Windows.Forms.Button()
       ' Add the button to the collection's internal list.
       Me.List.Add(aButton)
       ' Add the button to the controls collection of the form 
       ' referenced by the HostForm field.
       HostForm.Controls.Add(aButton)
       ' Set intial properties for the button object.
       aButton.Top = Count * 25
       aButton.Left = 100
       aButton.Tag = Me.Count
       aButton.Text = "Button " & Me.Count.ToString
       
       AddHandler aButton.Click, AddressOf ClickHandler
       
       Return aButton
    End Function
   

    Public Sub New(ByVal host as System.Windows.Forms.Form)
       HostForm = host
       Me.AddNewButton()
    End Sub
   
   Default Public ReadOnly Property Item(ByVal Index As Integer) As _
       System.Windows.Forms.Button
       Get
          Return CType(Me.List.Item(Index), System.Windows.Forms.Button)
       End Get
    End Property
   
   Public Sub Remove()
   ' Check to be sure there is a button to remove.
       If Me.Count > 0 Then
          ' Remove the last button added to the array from the host form 
          ' controls collection. Note the use of the default property in 
          ' accessing the array.
          HostForm.Controls.Remove(Me(Me.Count -1))
          Me.List.RemoveAt(Me.Count -1)
       End If
    End Sub

    Public Sub ClickHandler(ByVal sender As Object, ByVal e As _
       System.EventArgs)
       MessageBox.Show("you have clicked button " & CType(CType(sender, _
          System.Windows.Forms.Button).Tag, String))
    End Sub
    
    
End Class
