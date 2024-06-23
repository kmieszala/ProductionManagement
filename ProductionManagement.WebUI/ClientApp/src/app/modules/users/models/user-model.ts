import { DictModel } from "../../shared/models/dict-model";

export class UserModel {
    id: number;
    registeredDate: Date;
    activationDate?: Date;
    email: string;
    lastName: string;
    firstName: string;
    status: number;
    roles: DictModel[];
    password: string;

    showDetails: boolean;
    code: string = "****";
}
