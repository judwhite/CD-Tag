﻿using System.Windows;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
using CDTag.ViewModel.About;
using CDTag.ViewModel.Checksum;
using CDTag.ViewModel.Options;
using CDTag.ViewModel.Profile.EditProfile;
using CDTag.ViewModel.Profile.NewProfile;
using CDTag.ViewModel.Tag;
using CDTag.ViewModel.Tag.EditTag;
using CDTag.ViewModel.Tag.MassTag;
using CDTag.ViewModel.Tag.TagAlbum;
using CDTag.ViewModel.Tools;
using CDTag.Views;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace CDTag
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterInstance(typeof(IApp), Application.Current);
            Container.RegisterInstance(typeof(ITagViewModel), Container.Resolve<TagViewModel>());
            
            Container.RegisterType(typeof(IDirectoryController), typeof(DirectoryController));
            Container.RegisterType(typeof(IAboutViewModel), typeof(AboutViewModel));
            Container.RegisterType(typeof(IID3v2ViewModel), typeof(ID3v2ViewModel));
            Container.RegisterType(typeof(ITagAlbumViewModel), typeof(TagAlbumViewModel));
            Container.RegisterType(typeof(IMassTagViewModel), typeof(MassTagViewModel));
            Container.RegisterType(typeof(IEditProfileViewModel), typeof(EditProfileViewModel));
            Container.RegisterType(typeof(INewProfileViewModel), typeof(NewProfileViewModel));
            Container.RegisterType(typeof(ISplitCueViewModel), typeof(SplitCueViewModel));
            Container.RegisterType(typeof(IEncodingInspectorViewModel), typeof(EncodingInspectorViewModel));
            Container.RegisterType(typeof(IOptionsViewModel), typeof(OptionsViewModel));
            Container.RegisterType(typeof(IChecksumViewModel), typeof(ChecksumViewModel));
            Container.RegisterType(typeof(IVerifyEACLogViewModel), typeof(VerifyEACLogViewModel));

            Unity.Container = Container;
        }

        protected override DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();

            return shell;
        }
    }
}
