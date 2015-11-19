namespace Source.Model

module ButtonGame =

    type Button (value : int)   = 
        new ()                  = Button(0)
        member __.Click ()      = Button((value % 4) + 1)
        member __.Value         = value
        override __.ToString () = value.ToString()