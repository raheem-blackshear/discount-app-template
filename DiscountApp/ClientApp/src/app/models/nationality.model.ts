
export class Nationality {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, name?: string) {

        this.nationalityId = id;
        this.nationalityName = name;  

    }
    public nationalityId: string;
    public nationalityName: string;
    public isActive: boolean;

}
