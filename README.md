# InputValidationSample
Sample of Input Validation for WinUI 3.0

Proposal: https://github.com/microsoft/microsoft-ui-xaml/issues/179

Spec: https://github.com/microsoft/microsoft-ui-xaml-specs/pull/26


## Workaround

There is an issue in the alpha causing the default `TextBox` template to not contain the right components to display the visuals.

There is a workaround in App.xaml for this, which re-defines the implicity `TextBox` style, which contains the correct template.