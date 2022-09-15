import { TankParts } from "./tank-parts";

export class TankModel {
  id: number;
  name: string;
  productionDays: number;
  active: boolean;
  description: string;

  parts: TankParts[];

  showDetails: boolean = false;
}
