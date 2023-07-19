export class Age {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, name?: string, value?: string) {

        this.ageId = id;
        this.ageName = name;       
    }

    public ageId: string;
    public ageName: string;
    public isActive: boolean;

}
