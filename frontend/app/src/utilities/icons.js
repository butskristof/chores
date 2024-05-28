import * as IonIcons from 'oh-vue-icons/icons/io';
import * as LineAwesome from 'oh-vue-icons/icons/la';
import * as PrimeIcons from 'oh-vue-icons/icons/pr';

export const ICONS = Object.values({ ...IonIcons, ...LineAwesome, ...PrimeIcons });
export const ICON_NAMES = ICONS.map((i) => i.name);
