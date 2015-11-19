namespace Tests.Model


module ButtonGameProps =

    open NUnit.Framework
    open FsCheck
    open Source.Model.ButtonGame
    
    //Needed to enable NCrunch
    [<Test>]
    [<Ignore>]
    let ignoreMe() = ()

    let ``button value matches our model value`` 
        (button:Button, valueOnButton:int) =
            valueOnButton = button.Value |>  Prop.ofTestable

    let spec =
        let click =
            {
                new Command<Button,int>() with
                //Executes the command on the real model
                member __.RunActual button = button.Click()
                //Executes the command on the simplified model
                member __.RunModel valueOnButton = (valueOnButton % 4) + 1
                //Post condition that should hold after the command is run
                member __.Post (button, value) = ``button value matches our model value``(button, value)
                override __.ToString() = "Click" 
            }
        { 
            new ICommandGenerator<Button,int> with
            //Initialization of the real model
            member __.InitialActual = Button()
            //Initial state of the simplified model
            member __.InitialModel = 0
            //Generate a random number of commands
            member __.Next _ = Gen.elements [click]
        } |> Command.toProperty

    [<Test>]
    let verifyAll () = 
        Check.VerboseThrowOnFailure spec
