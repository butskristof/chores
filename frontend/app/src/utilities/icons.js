import * as LineAwesome from 'oh-vue-icons/icons/la';

export const ICONS = Object.values({
  ...LineAwesome,
});
export const ICON_NAMES = ICONS.map((i) => i.name);
