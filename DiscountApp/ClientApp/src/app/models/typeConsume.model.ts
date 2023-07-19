
export class TypeConsume {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: string, name?: string, value?: string) {

        this.typeConsumeId = id;
        this.typeConsumeName = name;
        this.typeConsumeValue = value;


    }

    public typeConsumeId: string;
    public typeConsumeName: string;
    public typeConsumeValue: string;
    public isActive: boolean;

}
