import { Routes } from '@angular/router';
import { MainLayout } from './layouts/main/layout';

export const routes: Routes = [
    {
        path: '',
        component: MainLayout,
        children: [
            { path: '', redirectTo: '/produtos', pathMatch: 'full' },
            {
                path: 'produtos',
                loadChildren: () => import('./pages/products/product.routes').then(m => m.PRODUCT_ROUTES),
            },
            {
                path: 'notas',
                loadChildren: () => import('./pages/invoices/invoice.routes').then(m => m.INVOICE_ROUTES),
            },
        ]
    }
];