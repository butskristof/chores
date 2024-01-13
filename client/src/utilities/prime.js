import PrimeVue from 'primevue/config';
import DialogService from 'primevue/dialogservice';
import ConfirmationService from 'primevue/confirmationservice';
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import InlineMessage from 'primevue/inlinemessage';
import Toolbar from 'primevue/toolbar';
import Textarea from 'primevue/textarea';
import Calendar from 'primevue/calendar';
import Card from 'primevue/card';
import Tag from 'primevue/tag';
import RadioButton from 'primevue/radiobutton';
import InputSwitch from 'primevue/inputswitch';
import Sidebar from 'primevue/sidebar';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';

export const setupPrimeVue = (app) => {
  app.use(PrimeVue);
  app.use(DialogService);
  app.use(ConfirmationService);
  [
    Dialog,
    Button,
    InputText,
    InlineMessage,
    Toolbar,
    Textarea,
    Calendar,
    Card,
    Tag,
    RadioButton,
    InputSwitch,
    Sidebar,
    DataTable,
    Column,
  ].forEach((component) => app.component(component.name, component));
};
