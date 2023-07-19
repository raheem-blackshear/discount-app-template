export class Help {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, name?: string, value?: string) {

        this.helpId = id;
        this.helpName = name;       
    }

    public helpId: string;
    public helpName: string;
    public isActive: boolean;

}
