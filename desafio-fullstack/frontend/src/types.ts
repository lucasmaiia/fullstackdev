export type LeadStatus = 'New' | 'Accepted' | 'Declined';

export interface Lead {
  id: number;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  dateCreated: string;
  suburb: string;
  category: string;
  description: string;
  price: number;
  status: LeadStatus;
}
